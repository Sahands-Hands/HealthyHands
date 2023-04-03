using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Radzen.Blazor;
using System.Security.Claims;
using Umbraco.Core.Services.Implement;


namespace HealthyHands.Client.Pages
{
    public partial class Weights
    {
        // Initialize the newWeightDate variable to the current date
        DateTime newWeightDate = DateTime.Today;
        private UserWeight newWeight = new UserWeight { Weight = 0, WeightDate = DateTime.Now };
        private string editingWeightId;
        private double editingWeight;
        private DateTime editingWeightDate;
        RadzenGrid<UserWeight> grid;
        bool showInputForm = false;


        [Inject]
        public HttpClient _httpClient { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWeightHttpRepository WeightHttpRepository { get; set; }

        public UserWeightDto UserWeightDto { get; set; } = new UserWeightDto();
        public UserDto User { get; set; } = new UserDto();
        private string warningMessage = "";

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userAuth = authState.User.Identity;
            if (userAuth is not null && userAuth.IsAuthenticated)
            {
                try
                {
                    await FetchData();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }
       

        private async Task AddWeight()
        {
            if (newWeight.Weight <= 0)
            {
                warningMessage = "Weight should be greater than zero.";
                return;
            }
            else
            {
                warningMessage = "";
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWeightDto = new UserWeightDto
            {
                Weight = newWeight.Weight,
                WeightDate = newWeightDate,
                ApplicationUserId = userId
            };

            var result = await WeightHttpRepository.AddWeight(userWeightDto);

            newWeight = new UserWeight { Weight = 0, WeightDate = DateTime.Now };
            newWeightDate = DateTime.Today;

            showInputForm = false;
            

            await FetchData();
        }

        private async Task FetchData()
        {
            User = await WeightHttpRepository.GetWeights();

            grid.Reload();
        }

        private async Task DeleteWeight(string id)
        {
            await WeightHttpRepository.DeleteWeight(id);
            var weightToRemove = User.UserWeights.FirstOrDefault(w => w.UserWeightId == id);
            if (weightToRemove != null)
            {
                User.UserWeights.Remove(weightToRemove);
            }

            await FetchData();
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

            // Update the UI to reflect the changes
            StateHasChanged();
        }

        private void Cancel()
        {
            newWeight = new UserWeight { Weight = 0, WeightDate = DateTime.Now };
            newWeightDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";

            // Remove the new weight from the list
            User.UserWeights.Remove(newWeight);
        }

        private async Task Save(string userWeightId)
        {
            // Create a new UserWeightDto object with the updated values of the weight entry
            var userWeightDto = new UserWeightDto
            {
                UserWeightId = userWeightId,
                Weight = editingWeight,
                WeightDate = editingWeightDate,
                ApplicationUserId = User.Id
            };

            // Pass the UserWeightDto object to the WeightHttpRepository service to update the weight entry in the database
            var result = await WeightHttpRepository.UpdateWeight(userWeightDto);

            // Update the corresponding weight entry in the UserWeights property of the User object with the new values
            var weightToUpdate = User.UserWeights.FirstOrDefault(w => w.UserWeightId == userWeightId);
            if (weightToUpdate != null)
            {
                weightToUpdate.Weight = editingWeight;
                weightToUpdate.WeightDate = editingWeightDate;
            }

            // Reset the editing state to exit edit mode
            editingWeightId = null;
            editingWeight = 0;
            editingWeightDate = DateTime.Now;

            // Update the UI to reflect the changes
            StateHasChanged();
        }



        private void addNewRow()
        {
            showInputForm = true;

            // Create a new instance of the UserWeight class to hold the input data
            newWeight = new UserWeight { Weight = 0, WeightDate = DateTime.Now };
            newWeightDate = DateTime.Today;
        }

    }
    }




