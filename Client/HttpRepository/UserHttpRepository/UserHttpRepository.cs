using HealthyHands.Shared.Models;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace HealthyHands.Client.HttpRepository.UserRepository
{
    public class UserHttpRepository : IUserHttpRepository
    {
        private readonly HttpClient _httpClient;
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
            var responseMessage = await _httpClient.GetAsync("user");
            var content = await ValidateContent(responseMessage).ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return user;
        }

        /// <summary>
        /// A method that uses the HTTP client to send a PUT request to the "user/update", the request body is populated with provided UserDto
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>returns the response of the updated information provided by the user</returns>
        public async Task<bool> UpdateUserInfo(UserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UserDto>("user/update", userDto);
            return response.IsSuccessStatusCode;
        }
        
        private HttpContent ValidateContent(HttpResponseMessage response)
        {
            if(string.IsNullOrEmpty(response.Content?.ReadAsStringAsync().Result))
            {
                return response.Content= new StringContent("null",Encoding.UTF8, MediaTypeNames.Application.Json);
            }
            else
            {
                return response.Content;
            }
        }
    }
}
