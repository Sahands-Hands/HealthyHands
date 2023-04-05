using HealthyHands.Client.HttpRepository.UserRepository;
using HealthyHands.Client.HttpRepository.WeightHttpRepository;
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
        [Inject] IWeightHttpRepository WeightHttpRepository { get; set; }
        [Inject] IUserHttpRepository UserHttpRepository { get; set; }

        public UserDto User { get; set; } = new();
        public string Goal { get; set; } = "maintain";
        public double Calories { get; set; }

        public UserWeightDto[] Weights { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (userAuth is not null && userAuth.IsAuthenticated)
            {
                try
                {
                    User = await UserHttpRepository.GetUserInfo();
                    Weights = await WeightHttpRepository.GetWeights();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }

        //public void CalculateCalories()
        //{
        //    int gender = User.Gender;
        //    int weight = User.Weight;
        //    int height = User.Height;
        //    DateTime? birthDay = User.BirthDate;
        //    int age = GetAge(birthDay);
        //    Calories = CalculateRecommendedCalories(gender, weight, height, age, Goal);
        //}

        private double CalculateRecommendedCalories(int gender, int weight, int height, int age, string goal)
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



