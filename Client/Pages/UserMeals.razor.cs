using HealthyHands.Client.HttpRepository.MealHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using Radzen;
using Radzen.Blazor;
namespace HealthyHands.Client.Pages
{
    public partial class UserMeals
    {
        //New UserMeals
        private UserMeal newMeal = new UserMeal { MealName = "", MealDate = DateTime.Now, Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sugar = 0 };
        private string newMealName;
        private DateTime newMealDate = DateTime.Today;
        private int? newCalories;
        private int? newProtein;
        private int? newCarbs;
        private int? newFat;
        private int? newSugar;
        //Edit UserMeals
        private string editMealId;
        private string? editMealName;
        private DateTime editMealDate;
        private int? editCalories;
        private int? editProtein;
        private int? editCarbs;
        private int? editFat;
        private int? editSugar;

        private DateTime? Today = DateTime.Today;
        private void ChangeDate(DateTime? value)
        {
            Today = value;
        }

        private string warningMessage = "";
        bool showInputForm = false;

        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        IMealsHttpRepository MealsHttpRepository { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

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
                    usermeals = User.UserMeals;
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
            else
            {
                // Redirect the user to the default weight feature page

                NavigationManager.NavigateTo("/defaultusermeals");


            }
        }

        RadzenDataGrid<UserMeal> grid;
        IEnumerable<UserMeal> usermeals;
        UserMeal mealToInsert;
        UserMeal mealsToUpdate;

        void Reset()
        {
            mealToInsert = null;
            mealsToUpdate = null;
        }

        async Task InsertRow()
        {
            mealToInsert = new UserMeal();
            mealToInsert.MealDate = DateTime.Today;
            await grid.InsertRow(mealToInsert);
        }

        private async Task FetchData()
        {
            User = await MealsHttpRepository.GetMeals();
            usermeals = User.UserMeals;
        }

        private async Task OnCreateRow(UserMeal meal)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userMealDto = new UserMealDto
            {
                MealName = meal.MealName,
                MealDate = meal.MealDate,
                Calories = meal.Calories,
                Protein = meal.Protein,
                Carbs = meal.Carbs,
                Fat = meal.Fat,
                Sugar = meal.Sugar,
                ApplicationUserId = meal.ApplicationUserId
            };

            var result = await MealsHttpRepository.AddMeals(userMealDto);

            // Reset the input fields
            newMealName = "";
            newMealDate = Today ?? DateTime.Today;
            newCalories = 0;
            newProtein = 0;
            newCarbs = 0;
            newFat = 0;
            newSugar = 0;

            StateHasChanged();

            

            mealToInsert = null;
        }
        private async Task OnUpdateRow(UserMeal meal)
        {
            
            if (meal == mealToInsert)
            {
                mealToInsert = null;
            }
            mealsToUpdate = null;

            UserMealDto userMeal = new UserMealDto
            {
                UserMealId = meal.UserMealId,
                MealName = meal.MealName,
                MealDate = meal.MealDate,
                Calories = meal.Calories,
                Protein = meal.Protein,
                Carbs = meal.Carbs,
                Fat = meal.Fat,
                Sugar = meal.Sugar,
                ApplicationUserId = meal.ApplicationUserId
            };

            var result = await MealsHttpRepository.UpdateMeals(userMeal);

            
        }
        private void addNewRow()
        {
            showInputForm = true;

            // Create a new instance of the UserWeight class to hold the input data
            newMeal = new UserMeal();
            newMealName = "";
            newMealDate = DateTime.Today;
            newCalories = 0;
            newProtein = 0;
            newCarbs = 0;
            newFat = 0;
            newSugar = 0;
        }
        async Task EditRow(UserMeal meals)
        {
            mealsToUpdate = meals;
            await grid.EditRow(meals);
        }

        async Task SaveRow(UserMeal meal)
        {
            await grid.UpdateRow(meal);

        }
        void CancelEdit(UserMeal meal)
        {
            if (meal == mealToInsert)
            {
                mealToInsert = null;
            }
            mealsToUpdate = null;

            grid.CancelEditRow(meal);

            UserMeal newMeal = new UserMeal { MealName = "", MealDate = DateTime.Today, Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sugar = 0 };
            newMealDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";
        }
        async Task DeleteRow(UserMeal meal)
        {
            var result = await MealsHttpRepository.DeleteMeals(meal.UserMealId);

            if (result)
            {
                // The weight was deleted successfully
                await FetchData();
                await grid.Reload();
            } else
            {
                grid.CancelEditRow(meal);
            }
        }
    }
}
