using HealthyHands.Shared.Models;

namespace HealthyHands.Server.Data.Repository.AdminRepository;

public interface IAdminRepository : IDisposable
{
    Task<List<UserDto>> GetAllUsers();

    Task<bool> UserExists(string userId);

    Task LockoutUser(string userId);

    Task ResetUserPassword(string userId);

    Task ChangeUserRoleToAdmin(string userId);

    Task ChangeUserRoleToUser(string userId);

    Task Save();

}