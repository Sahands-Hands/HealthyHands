using HealthyHands.Shared.Models;
namespace HealthyHands.Client.HttpRepository.UserRepository
{
    public interface IUserHttpRepository
    {
        Task<UserDto> GetUserInfo();
        //Task<UserDto> UpdateUserInfo();
    }
}
