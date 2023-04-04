using HealthyHands.Client.HttpRepository.AdminHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HealthyHands.Client.Pages
{
    public partial class Admin
    {
        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IAdminHttpRepository AdminHttpRepository { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        
        private string editingUserId;
        private double editingUser;
     
        private List<UserDto>? UsersList { get; set; }
 
        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    UsersList = await AdminHttpRepository.GetAllUsers();
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }  
            }
        }
     
        private async Task PromoteToAdmin(string userId)
        {
            var success = await AdminHttpRepository.SetUserRoleAdmin(userId);
            if (success)
            {
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Failed");
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
    }
}

