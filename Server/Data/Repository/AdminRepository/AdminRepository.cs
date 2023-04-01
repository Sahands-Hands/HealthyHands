using HealthyHands.Client;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.AdminRepository;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private bool _disposed;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _disposed = false;
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        List<UserDto> usersDtoList = new List<UserDto>();
        var usersList = await _userManager.Users.Select(u => u).ToListAsync();

        foreach (var u in usersList)
        {
            var isAdmin = await _userManager.IsInRoleAsync(u, "Admin");
            usersDtoList.Add(
                new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsAdmin = isAdmin,
                    LockoutEnabled = u.LockoutEnabled,
                });
        }
        
        return new List<UserDto>();
    }

    public async Task<bool> UserExists(string userId)
    {
        
        return true;
        
    }

    public async Task LockoutUser(string userId)
    {
        var userToLockout = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        userToLockout.LockoutEnabled = true;
        _context.Users.Update(userToLockout);
    }

    public async Task ResetUserPassword(string userId)
    {
        var userToResetPassword = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(userToResetPassword);
        await _userManager.ResetPasswordAsync(userToResetPassword, passwordResetCode, "2rx9j=Ik*BctHQ=");
    }

    public async Task ChangeUserRoleAdmin(string userId)
    {
        var oldUser = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        var oldRoleId = await _userManager.GetRolesAsync(oldUser);
        var oldRoleName = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Id == oldRoleId[0]);

        if (oldRoleName.Name != "Admin")
        {
            await _userManager.RemoveFromRoleAsync(oldUser, oldRoleName.Name);
            await _userManager.AddToRoleAsync(oldUser, "Admin");
        }
    }

    public async Task ChangeUserRoleUser(string userId)
    {
        var oldUser = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        var oldRoleId = await _userManager.GetRolesAsync(oldUser);
        var oldRoleName = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Id == oldRoleId[0]);

        if (oldRoleName.Name != "User")
        {
            await _userManager.RemoveFromRoleAsync(oldUser, oldRoleName.Name);
            await _userManager.AddToRoleAsync(oldUser, "User");
        }
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Logic to dispose the <see cref="UserManager{TUser}"/>.
    /// </summary>
    /// <param name="disposing">If true, disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
                _userManager.Dispose();
                _roleManager.Dispose();
            }
        }

        _disposed = true;
    }

    /// <summary>
    /// Disposes the <see cref="ApplicationDbContext"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    
}