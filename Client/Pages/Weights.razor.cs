using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Radzen.Blazor;
using System.Security.Claims;
using HealthyHands.Client.HttpRepository.WeightHttpRepository;


namespace HealthyHands.Client.Pages
{
    public partial class Weights
    {
     
        [Inject]
        public HttpClient _httpClient { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWeightHttpRepository WeightHttpRepository { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public UserDto CurrentUser { get; set; } = new UserDto();
        public UserWeightDto UserWeight { get; set; } = new();
        

        RadzenDataGrid<UserWeight> grid;
        RadzenChart chart;
        IEnumerable<UserWeight> weights;
        UserWeight weightToInsert;
        UserWeight weightToUpdate;


        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    CurrentUser = await WeightHttpRepository.GetWeights();
                    weights = CurrentUser.UserWeights;
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                } 
            }
            else
            {
                NavigationManager.NavigateTo("/defaultuserweights");
            }
        }

        void Reset()
        {
            weightToInsert = null;
            weightToUpdate = null;
        }

        async Task InsertRow()
        {
            weightToInsert = new UserWeight();
            weightToInsert.WeightDate = DateTime.Today;
            await grid.InsertRow(weightToInsert);
        }

        private async Task FetchData()
        {
            CurrentUser = await WeightHttpRepository.GetWeights();
            weights = CurrentUser.UserWeights;
        }

        private async Task OnCreateRow(UserWeight weight)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var userWeightDto = new UserWeightDto
            {

                Weight = weight.Weight,
                WeightDate = weight.WeightDate,
                ApplicationUserId = userId
            };
            var result = await WeightHttpRepository.AddWeight(userWeightDto);

            await FetchData();
            await chart.Reload();
            StateHasChanged();
            weightToInsert = null;
        }

        private async Task OnUpdateRow(UserWeight weight)
        {
            if (weight == weightToInsert)
            {
                weightToInsert = null;
            }
            weightToUpdate = null;

            UserWeightDto userWeight = new UserWeightDto
            {
                UserWeightId = weight.UserWeightId,
                Weight = weight.Weight,
                WeightDate = weight.WeightDate,
                ApplicationUserId = weight.ApplicationUserId,

            };

            var result = await WeightHttpRepository.UpdateWeight(userWeight);
            await FetchData();
            await chart.Reload();
        }



        async Task EditRow(UserWeight weight)
        {
            weightToUpdate = weight;
            await grid.EditRow(weight);
        }

        async Task SaveRow(UserWeight weight)
        {
            await grid.UpdateRow(weight);
            await chart.Reload();

        }
        void CancelEdit(UserWeight weight)
        {
            if (weight == weightToInsert)
            {
                weightToInsert = null;
            }
            weightToUpdate = null;

            grid.CancelEditRow(weight);
        }

        async Task DeleteRow(UserWeight weight)
        {
            var result = await WeightHttpRepository.DeleteWeight(weight.UserWeightId);
            if (result)
            {
                await FetchData();
                await grid.Reload();
                await chart.Reload();

            }
            else
            {
                grid.CancelEditRow(weight);
            }
        }

    

    }

}


