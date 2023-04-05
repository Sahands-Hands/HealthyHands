using HealthyHands.Client.HttpRepository.UserRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;




namespace HealthyHands.Client.Pages
{
    partial class UserCalories
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }



        [Inject] IUserHttpRepository UserHttpRepository { get; set; }

        public UserDto User { get; set; } = new();

        public string Goal { get; set; } = "maintain";

        public double calories { get; set; }





        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    User = await UserHttpRepository.GetUserInfo();

                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }

        public void CalculateCalories()
        {
            int gender = User.Gender;  // You can use the user's gender from the UserDto
            int weight = User.Weight;
            int height = User.Height;
            DateTime? birthDay = User.BirthDate;
            int age = GetAge(birthDay) ?? 30; // If the age is not available, assume it's 30

            calories = calculateCalories(gender, weight, height, age, Goal);
        }

        public double calculateCalories(int gender, int weight, int height, int age, string goal)
        {
            double rec;
            double height_cm = height * 2.54;
            double weight_kg = weight * 0.453;
            double calorieModifier = 1.0;

            // Determine the calorie modifier based on the goal
            if (goal.Equals("lose"))
            {
                calorieModifier = 0.9; // Reduce calorie intake by 10% for weight loss
            }
            else if (goal.Equals("gain"))
            {
                calorieModifier = 1.1; // Increase calorie intake by 10% for weight gain
            }

            // Calculate recommended calorie intake
            if (gender == 0)  //Assuming if gender is 0 then it is a male, otherwise need to change this
            {
                rec = 66.5 + (13.75 * (weight_kg)) + (5.003 * (height_cm)) - (6.75 * age);
                return (Math.Round(rec * calorieModifier, 2));
            }
            else
            {
                rec = 655.1 + (9.563 * (weight_kg)) + (1.85 * (height_cm)) - (4.676 * age);
                return (Math.Round(rec * calorieModifier, 2));
            }
        }

        public int? GetAge(DateTime? birthDay)
        {
            if (!birthDay.HasValue)
            {
                return null;
            }

            int age = DateTime.Today.Year - birthDay.Value.Year;

            if (birthDay > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
