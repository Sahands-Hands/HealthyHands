using HealthyHands.Client.HttpRepository.MealHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;

namespace HealthyHands.Client.Pages
{
    public partial class UserMeals
    {
        private string newMeal;
        private DateTime newMealDate = DateTime.Today;
        private int? newCalories;
        private int? newProtein;
        private int? newCarbs;
        private int? newFat;
        private int? newSugar;

        [Inject]
        public HttpClient _HttpClient { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IMealsHttpRepository MealsHttpRepository { get; set; }
        public UserDto User { get; set; } = new();
        public UserMealDto UserMeal { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    User = await MealsHttpRepository.GetMeals();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }
        private async Task AddUserMeals()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userMealDto = new UserMealDto
            {
                MealName = newMeal,
                MealDate = newMealDate,
                Calories = newCalories,
                Protein = newProtein,
                Carbs = newCarbs,
                Fat = newFat,
                Sugar = newSugar,
                ApplicationUserId = userId
            };

            var result = await MealsHttpRepository.AddMeals(userMealDto);

            // Reset the input fields
            newMeal = "";
            newMealDate = DateTime.Today;
            newCalories = 0;
            newProtein = 0;
            newCarbs = 0;
            newFat = 0;
            newSugar = 0;


            // Update the table data
            var userMeal = new UserMeal
            {
                MealName = userMealDto.MealName,
                MealDate = userMealDto.MealDate,
                Calories = userMealDto.Calories,
                Protein = userMealDto.Protein,
                Carbs = userMealDto.Carbs,
                Fat = userMealDto.Fat,
                Sugar = userMealDto.Sugar,
                ApplicationUserId = userMealDto.ApplicationUserId
            };
            User.UserMeals.Add(userMeal);
            StateHasChanged();
        }
    }
}
