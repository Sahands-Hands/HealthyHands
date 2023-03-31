// Filename: IUserRepository.cs

using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthyHands.Server.Data.Repository.UserRepository;

public interface IUserRepository : IDisposable
{
    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <param name="userId">The userId.</param>
    /// <returns>A <see cref="UserDto"/></returns>
    Task<UserDto> GetUser(string userId);

    /// <summary>
    /// Updates the user.
    /// </summary>
    /// <param name="userDto">The <see cref="UserDto"/>.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A Task.</returns>
    Task UpdateUser(UserDto userDto, string userId);

    /// <summary>
    /// Saves the context.
    /// </summary>
    Task Save();
}