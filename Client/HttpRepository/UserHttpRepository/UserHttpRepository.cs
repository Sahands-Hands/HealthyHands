using HealthyHands.Shared.Models;
using System.Net.Http.Json;
namespace HealthyHands.Client.HttpRepository.UserRepository
{
    public class UserHttpRepository : IUserHttpRepository
    {
        public readonly HttpClient _httpClient;
        public UserHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserDto> GetUserInfo()
        {
            UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("user");
            return user;
        }
    }
}
