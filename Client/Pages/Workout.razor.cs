using HealthyHands.Client.HttpRepository.MealHttpRepository;
using HealthyHands.Client.HttpRepository.WorkoutsRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Radzen;
using Radzen.Blazor;
namespace HealthyHands.Client.Pages
{ 
    public class WorkoutType
     {
         public int Value { get; set; }
         public string Name { get; set; }
     }

    public class WorkoutIntensity
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
    public partial class Workout
    {
        //New UserMeals
        private UserWorkout newWorkout = new UserWorkout { WorkoutName = "", WorkoutType = 1, Intensity = 1, Length = 0, WorkoutDate = DateTime.Today, CaloriesBurned = 0 };
        private string newWorkoutName;
        private int newWorkoutType;
        private int newIntensity;
        private int newLength;
        private DateTime newWorkoutDate = DateTime.Today;
        private int? newCaloriesBurned;

        private string editingWorkoutId;
        private string editingWorkoutName;
        private int editingWorkoutType;
        private int editingIntensity;
        private int editingLength;
        private DateTime editingWorkoutDate;
        private int? editingCaloriesBurned;

        //private DateTime? Today = DateTime.Today;
        //private void ChangeDate(DateTime? value)
        //{
        //    Today = value;
        //}

        private string warningMessage = "";
        bool showInputForm = false;

        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        IWorkoutsHttpRepository WorkoutsHttpRepository { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public UserDto CurrentUser { get; set; }
        public UserWorkoutDto UserWorkout { get; set; } = new();

        // 0 -> Bicycling
        // 1 -> Running
        // 2 -> Walking
        // 3 -> Swimming
        // 4 -> Skiing
        // 5 -> Climbing
        // 6 -> Strength/Weight Training

        //IEnumerable<UserWorkout> workout;

        public WorkoutType selectedWorkoutType { get; set; } = new();
        
        public IEnumerable<WorkoutType> WorkoutTypes { get; set; } = new List<WorkoutType>
        {
            new WorkoutType {Value = 0, Name = "Bicycling" },
            new WorkoutType {Value = 1, Name = "Running" },
            new WorkoutType {Value = 2, Name = "Walking"},
            new WorkoutType {Value = 3, Name = "Swimming"},
            new WorkoutType {Value = 4, Name = "Skiing"},
            new WorkoutType {Value = 5, Name = "Climbing"},
            new WorkoutType {Value = 6, Name = "Strenght/Weight Training"}
        };

        public IEnumerable<WorkoutIntensity> WorkoutIntensities { get; set; } = new List<WorkoutIntensity>
        {
            new WorkoutIntensity { Value = 0, Name = "Easy" },
            new WorkoutIntensity { Value = 1, Name = "Moderate" },
            new WorkoutIntensity { Value = 2, Name = "Vigorous" }
        };

        public int WorkoutTypeValue { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                   CurrentUser = await WorkoutsHttpRepository.GetWorkouts();
                   workouts = CurrentUser.UserWorkouts;
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
            else
            {
                // Redirect the user to the default weight feature page

                NavigationManager.NavigateTo("/defaultuserworkouts");

            }
        }
       

        //******************************************************************Radzen********************************************************************

        RadzenDataGrid<UserWorkout> grid;
        IEnumerable<UserWorkout> workouts;
        UserWorkout workoutToInsert;
        UserWorkout workoutToUpdate;

        void Reset()
        {
            workoutToInsert = null;
            workoutToUpdate = null;
        }

        async Task InsertRow()
        {
            workoutToInsert = new UserWorkout();
            workoutToInsert.WorkoutDate = DateTime.Today;
            await grid.InsertRow(workoutToInsert);
        }
        
        private async Task FetchData()
        {
            CurrentUser = await WorkoutsHttpRepository.GetWorkouts();
            workouts = CurrentUser.UserWorkouts;
        }
        
        private async Task OnCreateRow(UserWorkout workout)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWorkoutDto = new UserWorkoutDto
            {
                WorkoutName = workout.WorkoutName,
                WorkoutType = workout.WorkoutType,
                Intensity = workout.Intensity,
                Length = workout.Length,
                WorkoutDate = workout.WorkoutDate,
                CaloriesBurned = workout.CaloriesBurned ?? 0, // change to calories burned
                ApplicationUserId = workout.ApplicationUserId
            };

            var result = await WorkoutsHttpRepository.AddUserWorkout(userWorkoutDto);

            // Reset the input fields
            newWorkoutName = "";
            newWorkoutType = 0;
            newIntensity = 0;
            newLength = 0;
            newWorkoutDate = DateTime.Today;
            newCaloriesBurned = 0;


        
            StateHasChanged();

            
            workoutToInsert = null;
        }

        private async Task OnUpdateRow(UserWorkout workout)
        {
            if (workout == workoutToInsert)
            {
                workoutToInsert = null;
            }
            workoutToUpdate = null;

            UserWorkoutDto userWorkout = new UserWorkoutDto
            {
                UserWorkoutId = workout.UserWorkoutId,
                WorkoutName = workout.WorkoutName,
                WorkoutType = workout.WorkoutType,
                Intensity = workout.Intensity,
                Length = workout.Length,
                WorkoutDate = workout.WorkoutDate,
                CaloriesBurned = workout.CaloriesBurned ?? 0,
                ApplicationUserId = workout.ApplicationUserId
                
            };

            var result = await WorkoutsHttpRepository.UpdateWorkouts(userWorkout);

        }

       

        async Task EditRow(UserWorkout workout)
        {
            workoutToUpdate = workout;
            await grid.EditRow(workout);
        }

        async Task SaveRow(UserWorkout workout)
        {
            await grid.UpdateRow(workout);

        }
        void CancelEdit(UserWorkout workout)
        {
            if (workout == workoutToInsert)
            {
                workoutToInsert = null;
            }
            workoutToUpdate = null;

            grid.CancelEditRow(workout);

            UserWorkout newWorkout = new UserWorkout { WorkoutName = "", WorkoutType = 1, Intensity = 1, Length = 0, WorkoutDate = DateTime.Today, CaloriesBurned = 0 };
            newWorkoutDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";
        }

        async Task DeleteRow(UserWorkout workout)
        {
            // if (workout == workoutToInsert)
            // {
            //     workoutToInsert = null;
            // }
            //
            // if (workout == workoutToUpdate)
            // {
            //     workoutToUpdate = null;
            // }

            var result = await WorkoutsHttpRepository.DeleteWorkout(workout.UserWorkoutId);

            if (result)
            {
                await FetchData();
                await grid.Reload();
                
            }

            else
            {
                grid.CancelEditRow(workout);
            }
        }

        //**********Calories Burned**********//
        double calories_burned(double met, int weight, int duration)
        {
            
            return (Math.Round((met * 3.5 * ((weight * 0.453) / 200)) * duration, 2));
        }

        // 0 -> Bicycling
        // 1 -> Running
        // 2 -> Walking
        // 3 -> Swimming
        // 4 -> Skiing
        // 5 -> Climbing
        // 6 -> Strength/Weight Training
        Dictionary<int, List<double>> metDict = new Dictionary<int, List<double>> 
        { 
            { 0, new List<double> { 5.5, 7, 10.5 } }, 
            { 1, new List<double> { 8, 11.5, 16 } }, 
            { 2, new List<double> { 3, 3.5, 5.5 } }, 
            { 3, new List<double> { 6, 8, 10 } }, 
            { 4, new List<double> { 7, 8, 9 } }, 
            { 5, new List<double> { 7, 8, 11 } }, 
            { 6, new List<double> { 3.5, 5.5, 8 } } 
        };
    }

   

}