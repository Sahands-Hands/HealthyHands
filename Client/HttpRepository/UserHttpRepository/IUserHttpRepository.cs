//Filename: IUserHttpRepository.cs

using HealthyHands.Shared.Models;
namespace HealthyHands.Client.HttpRepository.UserRepository
{
    /// <summary>
    /// An interface that contains two asynchronous methods: "GetUserInfo()" which returns UserDto object, and "UpdateUserInfo()" which takes UserDto object and returns no value
    /// This repository is used to represent a repository for managing user information via HTTP requests
    /// </summary>
    public interface IUserHttpRepository
    {
        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<UserDto> GetUserInfo();

        /// <summary>
        /// Updates the user info.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <returns>A Task.</returns>
        Task<bool> UpdateUserInfo(UserDto userDto);
    }
}
