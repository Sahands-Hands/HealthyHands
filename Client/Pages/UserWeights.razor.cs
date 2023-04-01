using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading;

using System.Collections.Generic;
using System.Security.Claims;
using System.Timers;

namespace HealthyHands.Client.Pages
{
    public partial class UserWeights
    {
        private List<UserWeightDto> ChartData { get; set; }
        // Initialize the newWeightDate variable to the current date
        DateTime newWeightDate = DateTime.Today;
        private double newWeight;
        private string editingWeightId;
        private double editingWeight;
        private DateTime editingWeightDate;
        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
       

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWeightHttpRepository WeightHttpRepository { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        public UserWeightDto UserWeightDto { get; set; } = new();
        public UserDto User { get; set; } = new();
        private string warningMessage = "";


        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    await LoadChartData();
                    User = await WeightHttpRepository.GetWeights();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }
       

        private async Task LoadChartData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
  
        }



        private async Task AddWeight()
        {
            // Check if there is already an existing weight entry with the same date and time
            var existingWeight = User.UserWeights.FirstOrDefault(w => w.WeightDate == newWeightDate);
            if (existingWeight != null)
            {
                // Display a warning message to the user
                warningMessage = "Cannot add the same weight on the same day at the same time";
                return;
            }
            var previousWeight = User.UserWeights.OrderByDescending(w => w.WeightDate).FirstOrDefault();
            if (previousWeight != null && Math.Abs(newWeight - previousWeight.Weight) > 100)
            {
                // Display a warning message to the user
                warningMessage = "Weight cannot differ from the previous weight by more than 100";
                return;
            }
            // Check if newWeightDate is in the future
            if (newWeightDate > DateTime.Today)
            {
                // Display a warning message to the user
                warningMessage = "Weight date cannot be in the future";
                return;
            }

            // Your validation code here

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWeightDto = new UserWeightDto
            {
                Weight = newWeight,
                WeightDate = newWeightDate,
                ApplicationUserId = userId
            };

            var result = await WeightHttpRepository.AddWeight(userWeightDto);
            warningMessage = "";

            // Fetch updated data from server
            await FetchData();

            StateHasChanged();
        }
        private async Task FetchData()
        {
            // Fetch updated data from server
            var userDto = await WeightHttpRepository.GetWeights();

            // Update User.UserWeights with the updated data
            User.UserWeights = userDto.UserWeights;
        }


        
        private async Task Delete(string userWeightId)
        {
            var result = await WeightHttpRepository.DeleteWeight(userWeightId);

            if (result)
            {
                // The weight was deleted successfully
                var weightToDelete = User.UserWeights.FirstOrDefault(w => w.UserWeightId == userWeightId);
                if (weightToDelete != null)
                {
                    User.UserWeights.Remove(weightToDelete);
                    StateHasChanged();
                }
            }
            else
            {
                // There was an error deleting the weight
            }
        }

        private void Edit(string userWeightId)
        {
            var weightToEdit = User.UserWeights.FirstOrDefault(w => w.UserWeightId == userWeightId);
            if (weightToEdit != null)
            {
                editingWeightId = weightToEdit.UserWeightId;
                editingWeight = weightToEdit.Weight;
                editingWeightDate = weightToEdit.WeightDate;
            }
        }

        private void Cancel()
        {
            editingWeightId = null;
        }

        private async Task Save(string userWeightId)
        {
            // Update the weight in the database
            var userWeightDto = new UserWeightDto
            {
                UserWeightId = userWeightId,
                Weight = editingWeight,
                WeightDate = editingWeightDate,
                ApplicationUserId = User.Id
            };
            var result = await WeightHttpRepository.UpdateWeight(userWeightDto);

           // Update the weight in the table
            var weightToUpdate = User.UserWeights.FirstOrDefault(w => w.UserWeightId == userWeightId);
            if (weightToUpdate != null)
            {
                weightToUpdate.Weight = editingWeight;
                weightToUpdate.WeightDate = editingWeightDate;
                StateHasChanged();
            }

            // Reset the editing state
            editingWeightId = null;
            StateHasChanged();

        }


    }
}
    








