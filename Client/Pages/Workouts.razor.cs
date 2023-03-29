using HealthyHands.Client.HttpRepository.WorkoutsRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HealthyHands.Client.Pages
{
    public partial class Workouts
    {
        [Inject]
        public HttpClient _HttpClient { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWorkoutsHttpRepository WorkoutsHttpRepository { get; set;}

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
                catch(AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }

        }
        private async Task UpdateWorkouts()
        {
			//await WorkoutsHttpRepository.UpdateWorkouts(UserWorkout);
			NavigationManager.NavigateTo($"/updateWorkout/{UserWorkout.UserWorkoutId}"); //opens a new page to edit item need to create a new page gotta check with Ryan

			//<!-- <button type="button" @onclick="() => UpdateWorkouts(____)">Update</button> -->
		}
        private async Task AddUserWorkout()
        {
			await WorkoutsHttpRepository.AddUserWorkout(UserWorkout);
			NavigationManager.NavigateTo("/workouts");
		}
	}
}
