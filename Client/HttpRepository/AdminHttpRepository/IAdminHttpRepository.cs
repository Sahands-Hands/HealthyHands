using HealthyHands.Shared.Models;

namespace HealthyHands.Client.HttpRepository.AdminHttpRepository
{
    public interface IAdminHttpRepository
    {
        Task<List<UserDto>> GetAllUsers();
        Task<bool> LockoutUser(string userId);
        Task<bool> SetUserRoleAdmin(string userId);
        Task<bool> SetUserRoleUser(string userId);
        Task<bool> ResetUserPassword(string userId);
    }
}
