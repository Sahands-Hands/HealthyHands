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
        private double newWeight;
        private DateTime newWeightDate;
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


        public async Task AddWeight()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWeightDto = new UserWeightDto
            {
                Weight = newWeight,
                WeightDate = newWeightDate,
                ApplicationUserId = userId
            };

            var result = await WeightHttpRepository.AddWeight(userWeightDto);

            // Reset the input fields
            newWeight = 0;
            newWeightDate = DateTime.Today;
            await InvokeAsync(StateHasChanged);

            // Refresh the chart data
            await LoadChartData();
        }

        private async Task UpdateGraph()
        {
            await LoadChartData();
            StateHasChanged();
        }


        private async Task UpdateWeight()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWeightDto = new UserWeightDto
            {
                Weight = newWeight,
                WeightDate = newWeightDate,
                ApplicationUserId = userId
            };

            var result = await WeightHttpRepository.UpdateWeight(userWeightDto);

            if (result)
            {
                await LoadChartData();
                StateHasChanged();
            }
        }

        private async Task Delete(string userWeightId)
        {
            var result = await WeightHttpRepository.DeleteWeight(userWeightId);

           

            if (result)
            {
                // The weight was deleted successfully
            }
            else
            {
                // There was an error deleting the weight
            }
        }
    }
    }








