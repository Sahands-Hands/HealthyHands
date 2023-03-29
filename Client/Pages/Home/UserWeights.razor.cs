using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using System.Collections.Generic;

namespace HealthyHands.Client.Pages
{
    public partial class UserWeights
    {
        [Inject]
        public HttpClient _HttpClient { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IWeightHttpRepository WeightHttpRepository { get; set; }
       
        public UserDto User { get; set; } = new();

        protected async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    User = await WeightHttpRepository.GetWeights();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }
    }
}
