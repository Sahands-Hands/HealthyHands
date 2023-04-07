using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using Radzen;

namespace HealthyHands.Client.Pages
{
    partial class UserCalories
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] HttpRepository.WeightHttpRepository.IWeightHttpRepository WeightHttpRepository { get; set; }
        [Inject] HttpRepository.UserRepository.IUserHttpRepository UserHttpRepository { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }
        
        [Inject] NotificationService NotificationService { get; set; }





        public List<UserWeight> CurrentUserWeights { get; set; } = new List<UserWeight>();
        public UserDto? CurrentUser { get; set; }
        public string Goal { get; set; } = "none";
        public double Calories { get; set; }
        public int WeightObjective { get; set; }
        public string Message { get; set; } = "Awaiting Weight Goal";
        public string CalorieMessage { get; set; } = "";
        public string SaveMessage { get; set; } = "";
        private int SelectedGoal { get; set; } = 4;
        

        protected override async Task OnInitializedAsync()
        {
            var userAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (userAuth is not null && userAuth.IsAuthenticated)
            {
                try
                {
                    CurrentUser = await UserHttpRepository.GetUserInfo();
                    var userWeightsDto = await WeightHttpRepository.GetWeights();
                    CurrentUserWeights = userWeightsDto.UserWeights;
                    if (CurrentUser.WeightGoal == 0)
                    {
                        SelectedGoal = 0;
                        Goal = "lose";
                    } else if (CurrentUser.WeightGoal == 1)
                    {
                        SelectedGoal = 1;
                        Goal = "maintain";
                    } else if (CurrentUser.WeightGoal == 2)
                    {
                        SelectedGoal = 2;
                        Goal = "gain";
                    }

                    if (new List<int> { 0, 1, 2 }.Contains((int)CurrentUser.WeightGoal) &&
                        CurrentUser.CalorieGoal == null)
                    {
                        CurrentUser.CalorieGoal = CalculateCalories();
                        var result = await UserHttpRepository.UpdateUserInfo(CurrentUser);
                        ShowSuccessNotification();
                        await FetchData();
                    }

                    if (CalculateCalories() != CurrentUser.CalorieGoal)
                    {
                        CurrentUser.CalorieGoal = CalculateCalories();
                        var result = await UserHttpRepository.UpdateUserInfo(CurrentUser);
                        ShowSuccessNotification();
                        await FetchData();
                    }
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
            
            // if(CurrentUser.CalorieGoal != null && CurrentUser is not { CalorieGoal: 0, WeightGoal: null })
            // {
            //     Calories = (double)CurrentUser.CalorieGoal;
            //     WeightObjective = (int)CurrentUser.WeightGoal;
            //     
            //     
            // }
            //
            // if(WeightObjective == 0)
            // {
            //     Goal = "lose";
            //     Message = "Your saved weight goal is to lose weight";
            //     CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
            // }
            // else if(WeightObjective == 1)
            // {
            //     Goal = "maintain";
            //     Message = "Your saved weight goal is to maintain weight";
            //     CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
            // }
            // else if (WeightObjective == 2)
            // {
            //     Goal = "gain";
            //     Message = "Your saved weight goal is to gain weight";
            //     CalorieMessage = $"Your saved daily calorie intake is {Calories} calories";
            // }

        }

        private async Task FetchData()
        {
            CurrentUser = await UserHttpRepository.GetUserInfo();
            var userWeights = await WeightHttpRepository.GetWeights();
            CurrentUserWeights = userWeights.UserWeights;
            if (CurrentUser.WeightGoal == 0)
            {
                SelectedGoal = 0;
                Goal = "lose";
            } else if (CurrentUser.WeightGoal == 1)
            {
                SelectedGoal = 1;
                Goal = "maintain";
            } else if (CurrentUser.WeightGoal == 2)
            {
                SelectedGoal = 2;
                Goal = "gain";
            }
        }

        private void ChangeSelectedGoal(int selected)
        {
            SelectedGoal = selected;
        }

        private int CalculateCalories()
        {
            var gender = CurrentUser.Gender ?? 0;
            var weight = CurrentUserWeights.Last().Weight;
            var height = CurrentUser.Height ?? 0;
            var birthDay = CurrentUser.BirthDay;
            var age = GetAge(birthDay) ?? 20;
            return Convert.ToInt32(CalculateRecommendedCalories(gender, weight, height, age)) ;
        }

        // public void MaintainWeight()
        // {
        //     WeightObjective = 1;
        //     Goal = "maintain";
        //     
        //     if (CalculateCalories())
        //     {
        //         Message = $"Your desired goal is to {Goal} weight";
        //         CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        //     }
        // }
        // public void GainWeight()
        // {
        //     WeightObjective = 2;
        //     Goal = "gain";
        //     if (CalculateCalories())
        //     {
        //         Message = $"Your desired goal is to {Goal} weight";
        //         CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        //     }
        // }
        // public void LoseWeight()
        // {
        //     WeightObjective = 0;
        //     Goal = "lose";
        //     if (CalculateCalories())
        //     {
        //         Message = $"Your desired goal is to {Goal}</style> weight";
        //         CalorieMessage = $"To meet your goal, your recommended daily calorie intake is {Calories}";
        //     }
        // }

        private async Task OnUpdate()
        {
            CurrentUser.CalorieGoal = CalculateCalories();
            CurrentUser.WeightGoal = SelectedGoal;
            var result = await UserHttpRepository.UpdateUserInfo(CurrentUser);
            ShowSuccessNotification();
            await FetchData();
            
        }
        
        void ShowSuccessNotification()
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success Summary", Detail = "Success Detail", Duration = 4000 });
        }

        // public async void SaveCalories()
        // {
        //     int calorieGoal = (int)(Math.Round(Calories, MidpointRounding.AwayFromZero));
        //     var tempUserDto = new UserDto()
        //     {
        //         Id = UserInfo.Id,
        //         UserName = UserInfo.UserName,
        //         FirstName = UserInfo.FirstName,
        //         LastName = UserInfo.LastName,
        //         Height = UserInfo.Height,
        //         Gender = UserInfo.Gender,
        //         ActivityLevel = UserInfo.ActivityLevel,
        //         WeightGoal = WeightObjective,
        //         CalorieGoal = calorieGoal,
        //         BirthDay = UserInfo.BirthDay,
        //         LockoutEnabled = UserInfo.LockoutEnabled,
        //         IsAdmin = UserInfo.IsAdmin,
        //         UserMeals = UserInfo.UserMeals,
        //         UserWeights = UserInfo.UserWeights,
        //         UserWorkouts = UserInfo.UserWorkouts
        //     };
        //     var updated = false;
        //     if(Calories != 0)
        //     {
        //         updated = await UserHttpRepository.UpdateUserInfo(tempUserDto);
        //     }
        //     if (updated)
        //     {
        //         SaveMessage = "The calorie goal has been saved succesfully";
        //         await FetchData();
        //     }
        //     else
        //     {
        //         SaveMessage = "There was trouble updating the user please try again";
        //     }
        // }

        private double CalculateRecommendedCalories(int gender, double weight, int height, int age)
        {
            double heightCm = height * 2.54;
            double weightKg = weight * 0.453;
            double calorieModifier = 1.0;
            Console.WriteLine("Selected Goal: {0}", SelectedGoal);
            // Determine the calorie modifier based on the goal
            if (SelectedGoal == 0)
            {
                calorieModifier = 0.9; // Reduce calorie intake by 10% for weight loss
            }
            else if (SelectedGoal == 2)
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



