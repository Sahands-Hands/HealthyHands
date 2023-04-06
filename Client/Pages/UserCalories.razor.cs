using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;

namespace HealthyHands.Client.Pages
{
    partial class UserCalories
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] HttpRepository.WeightHttpRepository.IWeightHttpRepository WeightHttpRepository { get; set; }
        [Inject] HttpRepository.UserRepository.IUserHttpRepository UserHttpRepository { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }





        public UserDto User { get; set; } = new();
        public UserDto UserInfo { get; set; } = new();
        public string Goal { get; set; } = "maintain";
        public double Calories { get; set; }
        public string Message { get; set; } = "Awaiting Weight Goal";
        public string CalorieMessage { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            var userAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (userAuth is not null && userAuth.IsAuthenticated)
            {
                try
                {
                    User = await WeightHttpRepository.GetWeights();
                    UserInfo = await UserHttpRepository.GetUserInfo();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
            else
            {
                NavigationManager.NavigateTo("/defaultusercalories");
            }

        }

        public void CalculateCalories()
        {
            int weightLength;
            weightLength = User.UserWeights.Count;
            int gender = (int)UserInfo.Gender;
            double weight = User.UserWeights[weightLength-1].Weight;
            int height = (int)UserInfo.Height;
            DateTime? birthDay = UserInfo.BirthDay;
            int age = (int)GetAge(birthDay);
            Calories = CalculateRecommendedCalories(gender, weight, height, age, Goal);
        }

        public void MaintainWeight()
        {
            Goal = "maintain";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }
        public void GainWeight()
        {
            Goal = "gain";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }
        public void LoseWeight()
        {
            Goal = "lose";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }

        private double CalculateRecommendedCalories(int gender, double weight, int height, int age, string goal)
        {
            double heightCm = height * 2.54;
            double weightKg = weight * 0.453;
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
                double rec = 66.5 + (13.75 * weightKg) + (5.003 * heightCm) - (6.75 * age);
                return Math.Round(rec * calorieModifier, 2);
            }
            else
            {
                double rec = 655.1 + (9.563 * weightKg) + (1.85 * heightCm) - (4.676 * age);
                return Math.Round(rec * calorieModifier, 2);
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

    }



