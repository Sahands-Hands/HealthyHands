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
        public int WeightObjective { get; set; }
        public string Message { get; set; } = "Awaiting Weight Goal";
        public string CalorieMessage { get; set; } = "";
        public string SaveMessage { get; set; } = "";

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
            if((UserInfo.CalorieGoal != null && UserInfo.CalorieGoal != 0) && (UserInfo.WeightGoal != null))
            {
                Calories = (double)UserInfo.CalorieGoal;
                WeightObjective = (int)UserInfo.WeightGoal;
            }

            if(WeightObjective == 0)
            {
                Message = "Your saved weight goal is to lose weight";
                CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
            }
            else if(WeightObjective == 1)
            {
                Message = "Your saved weight goal is to maintain weight";
                CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
            }
            else if (WeightObjective == 2)
            {
                Message = "Your saved weight goal is to gain weight";
                CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
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
            WeightObjective = 1;
            Goal = "maintain";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }
        public void GainWeight()
        {
            WeightObjective = 2;
            Goal = "gain";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }
        public void LoseWeight()
        {
            WeightObjective = 0;
            Goal = "lose";
            CalculateCalories();
            Message = $"Your desired goal is to {Goal} weight";
            CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        }

        public async void SaveCalories()
        {
            int calorieGoal = (int)(Math.Round(Calories, MidpointRounding.AwayFromZero));
            var tempUserDto = new UserDto()
            {
                Id = UserInfo.Id,
                UserName = UserInfo.UserName,
                FirstName = UserInfo.FirstName,
                LastName = UserInfo.LastName,
                Height = UserInfo.Height,
                Gender = UserInfo.Gender,
                ActivityLevel = UserInfo.ActivityLevel,
                WeightGoal = WeightObjective,
                CalorieGoal = calorieGoal,
                BirthDay = UserInfo.BirthDay,
                LockoutEnabled = UserInfo.LockoutEnabled,
                IsAdmin = UserInfo.IsAdmin,
                UserMeals = UserInfo.UserMeals,
                UserWeights = UserInfo.UserWeights,
                UserWorkouts = UserInfo.UserWorkouts
            };
            var updated = false;
            if(Calories != 0)
            {
                updated = await UserHttpRepository.UpdateUserInfo(tempUserDto);
            }
            if (updated)
            {
                SaveMessage = "The calorie goal has been saved succesfully";
            }
            else
            {
                SaveMessage = "There was trouble updating the user please try again";
            }
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



