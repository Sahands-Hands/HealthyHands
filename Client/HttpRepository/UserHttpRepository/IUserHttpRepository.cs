using HealthyHands.Shared.Models;
namespace HealthyHands.Client.HttpRepository.UserRepository
{
    /// <summary>
    /// An interface that contains two asynchronous methods: "GetUserInfo()" which returns UserDto object, and "UpdateUserInfo()" which takes UserDto object and returns no value
    /// This repository is used to represent a repository for managing user information via HTTP requests
    /// </summary>
    public interface IUserHttpRepository
    {
        Task<UserDto> GetUserInfo();
        Task UpdateUserInfo(UserDto userDto);
    }
}
