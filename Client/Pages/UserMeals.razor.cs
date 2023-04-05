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
        }

        RadzenDataGrid<UserMeal> grid;
        IEnumerable<UserMeal> usermeals;
        UserMeal mealsToInsert;
        UserMeal mealsToUpdate;

        void Reset()
        {
            mealsToInsert = null;
            mealsToUpdate = null;
        }

        async Task InsertRow()
        {
            mealsToInsert = new UserMeal();
            await grid.InsertRow(mealsToInsert);
        }

        private async Task FetchData()
        {
            User = await MealsHttpRepository.GetMeals();

            await grid.Reload();
        }

        private async Task OnCreateRow(UserMeal meals)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userMealDto = new UserMealDto
            {
                MealName = meals.MealName,
                MealDate = Today ?? DateTime.Today,
                Calories = meals.Calories,
                Protein = meals.Protein,
                Carbs = meals.Carbs,
                Fat = meals.Fat,
                Sugar = meals.Sugar,
                ApplicationUserId = meals.ApplicationUserId
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

            

            mealsToInsert = null;
        }
        private async Task OnUpdateRow(UserMeal meals)
        {
            
            if (meals == mealsToInsert)
            {
                mealsToInsert = null;
            }
            mealsToUpdate = null;

            UserMealDto userMeal = new UserMealDto
            {
                UserMealId = meals.UserMealId,
                MealName = meals.MealName,
                MealDate = meals.MealDate,
                Calories = meals.Calories,
                Protein = meals.Protein,
                Carbs = meals.Carbs,
                Fat = meals.Fat,
                Sugar = meals.Sugar,
                ApplicationUserId = meals.ApplicationUserId
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

        async Task SaveRow(UserMeal meals)
        {
            await grid.UpdateRow(meals);

        }
        void CancelEdit(UserMeal meals)
        {
            if (meals == mealsToInsert)
            {
                mealsToInsert = null;
            }
            mealsToUpdate = null;

            grid.CancelEditRow(meals);

            UserMeal newMeal = new UserMeal { MealName = "", MealDate = DateTime.Today, Calories = 0, Protein = 0, Carbs = 0, Fat = 0, Sugar = 0 };
            newMealDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";
        }
        async Task DeleteRow(UserMeal meals)
        {
            if (meals == mealsToInsert)
            {
                mealsToInsert = null;
            }

            if (meals == mealsToUpdate)
            {
                mealsToUpdate = null;
            }

            var result = await MealsHttpRepository.DeleteMeals(meals.UserMealId);

            if (result)
            {
                // The weight was deleted successfully
                var mealToDelete = User.UserMeals.FirstOrDefault(w => w.UserMealId == meals.UserMealId);
                if (mealToDelete != null)
                {
                    User.UserMeals.Remove(mealToDelete);
                    await grid.Reload();
                    StateHasChanged();
                }
            }

            else
            {
                grid.CancelEditRow(meals);
                await grid.Reload();
            }

            StateHasChanged();
            await grid.Reload();
            await FetchData();
        }

    }
}
