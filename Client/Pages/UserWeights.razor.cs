using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading;

using System.Collections.Generic;
using System.Security.Claims;

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
            //if (newWeight <= 0)
            //{
            //    // Display a warning message to the user
            //    Console.WriteLine(" Weight must be greater than 0");

            //}
            //else if (newWeightDate > DateTime.Today)
            //{
            //    // Display a warning message to the user
            //    Console.WriteLine(" Weight date cannot be in the future");
            //}
            //else if (newWeightDate < DateTime.Today)
            //{
            //    // Display a warning message to the user
            //}
            //else
            //{
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var userWeightDto = new UserWeightDto
                {
                    Weight = newWeight,
                    WeightDate = newWeightDate,
                    ApplicationUserId = userId
                };

                var result = await WeightHttpRepository.AddWeight(userWeightDto);

                //// Reset the input fields
                //newWeight = 0;
                //newWeightDate = DateTime.Today;

                // Update the table data
                //var userWeight = new UserWeight
                //{
                //    Weight = userWeightDto.Weight,
                //    WeightDate = userWeightDto.WeightDate,
                //    ApplicationUserId = userWeightDto.ApplicationUserId
                //};
               // User.UserWeights.Add(userWeight);
                StateHasChanged();
            }
  //     }


        //private async Task UpdateWeight()
        //{
        //    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //    var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    var userWeightDto = new UserWeightDto
        //    {
        //        Weight = newWeight,
        //        WeightDate = newWeightDate,
        //        ApplicationUserId = userId
        //    };

        //    var result = await WeightHttpRepository.UpdateWeight(userWeightDto);

        //    if (result)
        //    {
        //        await LoadChartData();
        //        StateHasChanged();
        //    }
        //}

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
    








