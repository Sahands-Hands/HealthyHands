using HealthyHands.Client.HttpRepository.WorkoutsRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Collections.Generic;
using Radzen.Blazor;


namespace HealthyHands.Client.Pages
{
    public partial class Workouts
    {

        private string newWorkoutName;
        private int newWorkoutType;
        private int newIntensity;
        private int newLength;
        private DateTime newWorkoutDate = DateTime.Today;
        private int newCaloriesBurned;


        [Inject]
        public HttpClient _HttpClient { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWorkoutsHttpRepository WorkoutsHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public UserWorkoutDto UserWorkout { get; set; } = new();
        public UserDto User { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    User = await WorkoutsHttpRepository.GetWorkouts();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
                
            }

        }
        private async Task UpdateWorkout(string userWorkoutId)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Update the existing user workout with the new data
            var userWorkoutDto = new UserWorkoutDto
            {
                UserWorkoutId = UserWorkout.UserWorkoutId,
                WorkoutName = UserWorkout.WorkoutName,
                WorkoutType = newWorkoutType,
                Intensity = newIntensity,
                Length = newLength,
                WorkoutDate = newWorkoutDate,
                CaloriesBurned = newCaloriesBurned,
                ApplicationUserId = userId
            };

            var result = await WorkoutsHttpRepository.UpdateWorkouts(userWorkoutDto);

            if (result)
            {
                // Update the table data with the new values
                UserWorkout.WorkoutType = newWorkoutType;
                UserWorkout.Intensity = newIntensity;
                UserWorkout.Length = newLength;
                UserWorkout.WorkoutDate = newWorkoutDate;
                UserWorkout.CaloriesBurned = newCaloriesBurned;
                StateHasChanged();
            }
            else
            {
                // There was an error updating the workout
            }

            //var workoutToUpdate = User.UserWorkouts.FirstOrDefault(w => w.UserWorkoutId == userWorkoutId);
            //if (workoutToUpdate == null)
            //{
            //	// The workout to update wasn't found
            //	return;
            //}

            //var updatedUserWorkoutDto = new UserWorkoutDto
            //{
            //	UserWorkoutId = userWorkoutId,
            //	WorkoutName = workoutToUpdate.WorkoutName,
            //	WorkoutType = workoutToUpdate.WorkoutType,
            //	Intensity = workoutToUpdate.Intensity,
            //	Length = workoutToUpdate.Length,
            //	WorkoutDate = workoutToUpdate.WorkoutDate,
            //	CaloriesBurned = workoutToUpdate.CaloriesBurned,
            //	ApplicationUserId = workoutToUpdate.ApplicationUserId
            //};

            //var result = await WorkoutsHttpRepository.UpdateWorkouts(updatedUserWorkoutDto);

            //if (result)
            //{
            //	// The workout was updated successfully
            //	workoutToUpdate.WorkoutName = updatedUserWorkoutDto.WorkoutName;
            //	workoutToUpdate.WorkoutType = updatedUserWorkoutDto.WorkoutType;
            //	workoutToUpdate.Intensity = updatedUserWorkoutDto.Intensity;
            //	workoutToUpdate.Length = updatedUserWorkoutDto.Length;
            //	workoutToUpdate.WorkoutDate = updatedUserWorkoutDto.WorkoutDate;
            //	workoutToUpdate.CaloriesBurned = updatedUserWorkoutDto.CaloriesBurned;
            //	workoutToUpdate.ApplicationUserId = updatedUserWorkoutDto.ApplicationUserId;
            //	StateHasChanged();
            //}
            //else
            //{
            //	// There was an error updating the workout
            //}
        }


        private async Task AddWorkout()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWorkoutDto = new UserWorkoutDto
            {
                WorkoutName = newWorkoutName,
                WorkoutType = newWorkoutType,
                Intensity = newIntensity,
                Length = newLength,
                WorkoutDate = newWorkoutDate,
                CaloriesBurned = newCaloriesBurned,
                ApplicationUserId = userId
            };

            var result = await WorkoutsHttpRepository.AddUserWorkout(userWorkoutDto);

            // Reset the input fields
            newWorkoutName = "";
            newWorkoutType = 1;
            newIntensity = 1;
            newLength = 0;
            newWorkoutDate = DateTime.Today;
            newCaloriesBurned = 0;


            // Update the table data
            var userWorkout = new UserWorkout
            {
                WorkoutName = userWorkoutDto.WorkoutName,
                WorkoutType = userWorkoutDto.WorkoutType,
                Intensity = userWorkoutDto.Intensity,
                Length = userWorkoutDto.Length,
                WorkoutDate = userWorkoutDto.WorkoutDate,
                CaloriesBurned = userWorkoutDto.CaloriesBurned,
                ApplicationUserId = userWorkoutDto.ApplicationUserId
            };
            User.UserWorkouts.Add(userWorkout);
            StateHasChanged();
            this.isAdd = true; //for the protected stuff
        }

        private async Task DeleteWorkout(string userWorkoutId)
        {
            var result = await WorkoutsHttpRepository.DeleteWorkout(userWorkoutId);

            if (result)
            {
                // The weight was deleted successfully
                var workoutToDelete = User.UserWorkouts.FirstOrDefault(w => w.UserWorkoutId == userWorkoutId);
                if (workoutToDelete != null)
                {
                    User.UserWorkouts.Remove(workoutToDelete);
                    StateHasChanged();
                }
            }
            else
            {
                // There was an error deleting the weight
            }
            this.isDelete = false; // for the protected
        }

        //private void EditWorkout(string userWorkoutId)
        //{
        //	var workoutToEdit = User.UserWorkouts.FirstOrDefault(w => w.UserWorkoutId == userWorkoutId);
        //	if (workoutToEdit == null)
        //	{
        //		// The workout to edit wasn't found
        //		return;
        //	}

        //	// Set the input fields to the current values
        //	newWorkoutName = workoutToEdit.WorkoutName;
        //	newWorkoutType = workoutToEdit.WorkoutType;
        //	newIntensity = workoutToEdit.Intensity;
        //	newLength = workoutToEdit.Length;
        //	newWorkoutDate = workoutToEdit.WorkoutDate;
        //	newCaloriesBurned = workoutToEdit.CaloriesBurned;

        //	// Set the workout to edit
        //	UserWorkout = workoutToEdit;
        //}
        bool isNew = true;
        private async void HandleSubmit()
        {
            if (isNew)
            {
                await WorkoutsHttpRepository.UpdateWorkouts(UserWorkout);
            }
        }


        protected string SearchString { get; set; }
        protected Boolean isDelete = false;
        protected Boolean isAdd = false;
        protected string modelTitle { get; set; }
        protected async Task FilterWorkout()
        {
            await WorkoutsHttpRepository.GetWorkouts();
            if (SearchString != "")
            {
                User.UserWorkouts = User.UserWorkouts.Where(x => x.WorkoutName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
        }
        protected void closeModel()
        {
            this.isAdd = false;
            this.isDelete = false;
        }

        private void AddPopupWorkout()
        {
            UserWorkout = new UserWorkoutDto();
            this.modelTitle = "Add Workout";
            this.isAdd = true;
        }

    }
}
