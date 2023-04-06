using HealthyHands.Client.HttpRepository.AdminHttpRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using HealthyHands.Client.HttpRepository.UserRepository;
using Radzen;

namespace HealthyHands.Client.Pages
{
    public partial class AdminFeature
    {
        private string warningMessage = "";
        bool showInputForm = false;


        /// <summary>
        /// Create Class for Admin to select from in the dropdown
        /// </summary>
        public class SelectAdmin
        {
            public bool Value { get; set; }
            public string Name { get; set; }
        }
        /// <summary>
        /// Make IEnumerable of the SelectAdmin class so that it can be used as Data in .razor page with values
        /// </summary>
        public IEnumerable<SelectAdmin> Admin { get; set; } = new List<SelectAdmin>
        {
            new SelectAdmin {Value = true, Name = "Admin"},
            new SelectAdmin {Value = false, Name = "User"}
        };
        /// <summary>
        /// Create Class for LockedOut to select from in the dropdown
        /// </summary>
        public class SelectLockout
        {
            public bool Value { get; set; }
            public string Name { get; set; }
        }
        /// <summary>
        /// Create IEnumerable of the SelectLockout class so that it can be used as Data in .razor page with values
        /// </summary>
        public IEnumerable<SelectLockout> Lockouts { get; set; } = new List<SelectLockout>
        {
            new SelectLockout {Value = true, Name = "Locked"},
            new SelectLockout {Value = false, Name = "Unlocked"}
        };

        /// <summary>
        /// Create bool for ResetPassword so that it can be used as a checkbox in .razor
        /// </summary>
        public bool checkBoxValue = false;

        [Inject]
        public HttpClient _HttpClient { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        IAdminHttpRepository AdminHttpRepository { get; set; }
        public List<UserDto> Users { get; set; } = new();




        RadzenDataGrid<UserDto> grid;
        IEnumerable<UserDto> userinfo;
        UserDto infoToUpdate;
        List<UserDto> UserList = new();

        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                try
                {
                    Users = await AdminHttpRepository.GetAllUsers();
                    UserList = Users;
                    userinfo = Users;
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
            Users = await AdminHttpRepository.GetAllUsers();

            grid.Reload();
        }

        private async Task OnUpdateRow(UserDto user)
        {
            infoToUpdate = null;

            //UserDto userDto = new UserDto
            //{
            //    Id = user.Id,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    IsAdmin = user.IsAdmin,
            //    CalorieGoal = user.CalorieGoal,
            //    LockoutEnabled = user.LockoutEnabled

            //};

            if (user.IsAdmin == true)
            {
                try
                {

                    await AdminHttpRepository.SetUserRoleAdmin(user.Id);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }

            }
            else
            {
                try
                {
                    await AdminHttpRepository.SetUserRoleUser(user.Id);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }

            }

            if (user.LockoutEnabled == true)
            {
                try
                {

                    await AdminHttpRepository.LockoutUser(user.Id);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }
            else
            {
                try
                {
                    await AdminHttpRepository.UnlockUser(user.Id);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }
            }



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

            UserDto newInfo = new UserDto { Id = "", FirstName = "", LastName = "", IsAdmin = false, LockoutEnabled = false };
            //newMealDate = DateTime.Today;

            showInputForm = false;

            warningMessage = "";
        }

        //async Task ResetPWD(UserDto user)
        //{
        //    await AdminHttpRepository.ResetUserPassword(user.Id);
        //}

        protected async System.Threading.Tasks.Task ResetPWD(UserDto user)
        {
            // Ask for confirmation:
            var confirmResult = await DialogService.Confirm("Are you sure?", "Reset Password", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

            if (confirmResult.HasValue && confirmResult.Value)
            {
                try
                {
                    await AdminHttpRepository.ResetUserPassword(user.Id);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    exception.Redirect();
                }

            }

        }


    }


}