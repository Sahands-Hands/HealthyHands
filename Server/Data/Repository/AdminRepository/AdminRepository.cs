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
        var usersList = await _userManager.Users.Select(u =>
            new
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                LockoutEnabled = u.LockoutEnabled
            }).ToListAsync();

        return new List<UserDto>();
    }

    public async Task<bool> UserExists(string userId)
    {
        return true;}

    public async Task LockoutUser(string userId){}

    public async Task ChangeUserRoleAdmin(string userId){}
    
    public async Task ChangeUserRoleUser(string userId) {}

    public async void Save()
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