// Filename = UserRepository.cs

using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.UserRepository;

/// <summary>
/// The user repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userManager">The user manager.</param>
    public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
        _disposed = false;
    }

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>A Task.</returns>
    public async Task<UserDto> GetUser(string userId)
    {
        var userDto = await _userManager.Users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Height = u.Height,
            Gender = u.Gender,
            ActivityLevel = u.ActivityLevel,
            WeightGoal = u.WeightGoal,
            CalorieGoal = u.CalorieGoal,
            BirthDay = u.BirthDay,
        }).FirstOrDefaultAsync(u => u.Id == userId);

        return userDto;
    }

    /// <summary>
    /// Updates the user.
    /// </summary>
    /// <param name="userDto">The user dto.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A Task.</returns>
    public async Task UpdateUser(UserDto userDto, string userId)
    {
        var userToUpdate = await _userManager.Users.Select(u => u).FirstOrDefaultAsync(u => u.Id == userId);

        userToUpdate.FirstName = userDto.FirstName;
        userToUpdate.LastName = userDto.LastName;
        userToUpdate.Height = userDto.Height;
        userToUpdate.Gender = userDto.Gender;
        userToUpdate.ActivityLevel = userDto.ActivityLevel;
        userToUpdate.WeightGoal = userDto.WeightGoal;
        userToUpdate.CalorieGoal = userDto.CalorieGoal;
        userToUpdate.BirthDay = userDto.BirthDay;

        await _userManager.UpdateAsync(userToUpdate);
    }

    /// <summary>
    /// Saves the context.
    /// </summary>
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