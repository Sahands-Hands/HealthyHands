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
    private readonly RoleManager<IdentityRole> _roleManager;
    private bool _disposed;

    public AdminRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
        var adminRole = await _context.Roles.Select(ur => new {Id = ur.Id, Name = ur.Name}).FirstOrDefaultAsync(ur => ur.Name == "Admin");
        
        Console.WriteLine("Here is the admin role: Id = {0}, Name = {1}", adminRole.Id, adminRole.Name);
        foreach (var u in usersList)
        {
            IdentityUserRole<string>? userRole = new IdentityUserRole<string>();
            userRole = await _context.UserRoles.Select(ur => ur)
                .FirstOrDefaultAsync(ur => ur.UserId == u.Id);
            var isAdmin = false;
            if (userRole != null)
            {
                isAdmin = userRole.RoleId.Equals(adminRole.Id);
            }
            
            usersDtoList.Add(
                new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsAdmin = isAdmin,
                    LockoutEnabled = u.LockoutEnabled,
                });
        }
        
        return usersDtoList;
    }

    public async Task<bool> UserExists(string userId)
    {
        var user = await _userManager.Users.Select(u => u.Id).FirstOrDefaultAsync(u => u == userId);
        
        return (user == userId);
    }

    public async Task LockoutUser(string userId)
    {
        var userToLockout = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        await _userManager.SetLockoutEnabledAsync(userToLockout, true);
        await _userManager.SetLockoutEndDateAsync(userToLockout, DateTimeOffset.MaxValue);
    }

    public async Task UnlockUser(string userId)
    {
        var userToUnlock = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        await _userManager.SetLockoutEnabledAsync(userToUnlock, false);
        await _userManager.SetLockoutEndDateAsync(userToUnlock, DateTimeOffset.Now);
    }

    public async Task ResetUserPassword(string userId)
    {
        var userToResetPassword = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
        var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(userToResetPassword);
        await _userManager.ResetPasswordAsync(userToResetPassword, passwordResetCode, "2rx9j=Ik*BctHQ=");
    }

    // public async Task ChangeUserRoleToAdmin(string userId)
    // {
    //     var oldUser = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);
    //     var oldRoleId = await _userManager.GetRolesAsync(oldUser);
    //     var oldRoleName = await _context.Roles.SingleOrDefaultAsync(r => r.Id == oldRoleId[0]);
    //
    //     if (oldRoleName.Name != "Admin")
    //     {
    //         await _userManager.RemoveFromRoleAsync(oldUser, oldRoleName.Name);
    //         await _userManager.AddToRoleAsync(oldUser, "Admin");
    //     }
    // }
    
    public async Task ChangeUserRoleToAdmin(string userId)
    {
        var oldUser = await _userManager.FindByIdAsync(userId);
        await _userManager.AddToRoleAsync(oldUser, "admin");
    }

    public async Task ChangeUserRoleToUser(string userId)
    {
        var oldUser = await _userManager.FindByIdAsync(userId);
        await _userManager.RemoveFromRoleAsync(oldUser, "admin");
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