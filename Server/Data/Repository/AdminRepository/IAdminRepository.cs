using HealthyHands.Shared.Models;

namespace HealthyHands.Server.Data.Repository.AdminRepository;

public interface IAdminRepository : IDisposable
{
    Task<List<UserDto>> GetAllUsers();

    Task<bool> UserExists(string userId);

    Task LockoutUser(string userId);

    Task ChangeUserRoleAdmin(string userId);

    Task ChangeUserRoleUser(string userId);

    void Save();

}