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
        /// <summary>
        /// This method uses HTTP client to send a GET request to the API and retrieves the data in JSON format and deserialize it.
        /// </summary>
        /// <returns>UserDto object</returns>
        public async Task<UserDto> GetUserInfo()
        {
            UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("user");
            return user;
        }
        /// <summary>
        /// A method that uses the HTTP client to send a PUT request to the "user/update", the request body is populated with provided UserDto
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>returns the response of the updated information provided by the user</returns>
        public async Task UpdateUserInfo(UserDto userDto)
        {
            UserDto userdto = new UserDto();
            var response = await _httpClient.PutAsJsonAsync<UserDto>("user/update", userDto);
        }
    }
}
