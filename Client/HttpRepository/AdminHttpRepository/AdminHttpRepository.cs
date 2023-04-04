using HealthyHands.Shared.Models;
using System.Net.Http.Json;

namespace HealthyHands.Client.HttpRepository.AdminHttpRepository
{
    public class AdminHttpRepository : IAdminHttpRepository
    {
        private readonly HttpClient _httpClient;
        public AdminHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var userDtoList = await _httpClient.GetFromJsonAsync<List<UserDto>>("admin");
            
            return userDtoList;
        }

        public async Task<bool> LockoutUser(string userId)
        { 
            var response = await _httpClient.PutAsJsonAsync("admin/lockout", userId);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetUserRoleAdmin(string userId)
        { 
            var response = await _httpClient.PutAsJsonAsync($"admin/role/admin/{userId}", userId);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetUserRoleUser(string userId)
        {
            var response = await _httpClient.PutAsJsonAsync("admin/role/user", userId);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetUserPassword(string userId)
        {
            var response = await _httpClient.PutAsJsonAsync("admin/reset", userId);
            
            return response.IsSuccessStatusCode;
        }
    }
}
