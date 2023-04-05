using HealthyHands.Client.HttpRepository.UserRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using Radzen;
using Radzen.Blazor;
namespace HealthyHands.Client.Pages
{
    public partial class UserInfo
    {
        private string warningMessage = "";
        bool showInputForm = false;

        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        IUserHttpRepository UserHttpRepository { get; set; }
        public UserDto User { get; set; } = new();

        RadzenDataGrid<UserDto> grid;
        IEnumerable<UserDto> userinfo;
        UserDto infoToUpdate;
        List<UserDto> UserList = new();

        public class Gender 
        { 
            public int Value { get; set; }
            public string Name { get; set; }
        }
        public class ActivityLevel
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }
        public ActivityLevel selectedActivityLevel { get; set; } = new();
        public IEnumerable<ActivityLevel> ActivityTypes { get; set; } = new List<ActivityLevel>
        {
            new ActivityLevel {Value = 0, Name = "Light"},
            new ActivityLevel {Value = 1, Name = "Moderate"},
            new ActivityLevel {Value = 2, Name = "High"}
        };

        public Gender selectedGender { get; set; } = new();

        public IEnumerable<Gender> GenderTypes { get; set; } = new List<Gender>
        {
            new Gender {Value = 0, Name = "Male"},
            new Gender {Value = 1, Name = "Female"},
        };

        public int GenderTypeValue { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    User = await UserHttpRepository.GetUserInfo();
                    UserList.Add(User);
                    userinfo = UserList;
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
        }
        void Reset()
        {
            infoToUpdate = null;
        }
        private async Task FetchData()
        {
            User = await UserHttpRepository.GetUserInfo();

            grid.Reload();
        }
        private async Task OnUpdateRow(UserDto user)
        {
            infoToUpdate = null;

            UserDto userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Height = user.Height,
                Gender = selectedGender.Value,
                ActivityLevel = selectedActivityLevel.Value,
                WeightGoal = user.WeightGoal,
                CalorieGoal = user.CalorieGoal,
                Id = user.Id
            };
            var result = await UserHttpRepository.UpdateUserInfo(userDto);
            await FetchData();
        }
        async Task EditRow(UserDto user)
        {
            infoToUpdate = user;
            await grid.EditRow(user);
        }
        async Task SaveRow(UserDto user)
        {
            await grid.UpdateRow(user);

        }
        void CancelEdit(UserDto user)
        {
            
            infoToUpdate = null;

            grid.CancelEditRow(user);

            UserDto newInfo = new UserDto { FirstName = "", LastName= "", Height= 0, Gender=0, ActivityLevel=0,WeightGoal=0,CalorieGoal=0 };
            //newMealDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";
        }
    }
}
