using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using System.Security.Claims;

namespace HealthyHands.Client.HttpRepository.AdminHttpRepository
{
    public class AdminHttpRepository : IAdminHttpRepository
    {
        public readonly HttpClient _httpClient;
        public AdminHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            UserDto userDto = await _httpClient.GetFromJsonAsync<UserDto>("admin");
            return new List<UserDto>() { userDto };
            //return await JsonSerializer.DeserializeAsync<List<UserDto>>
            //    (await _httpClient.GetStreamAsync($"api/admin"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> LockoutUser(string userId)
        { 
            var response = await _httpClient.PutAsJsonAsync("admin/lockout", userId);
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SetUserRoleAdmin(string userId)
        { 
            var response = await _httpClient.PutAsJsonAsync("admin/role/admin", userId);
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SetUserRoleUser(string userId)
        {
            var response = await _httpClient.PutAsJsonAsync("admin/role/admin", userId);
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ResetUserPassword(string userId)
        {
            var response = await _httpClient.PutAsJsonAsync("admin/pwdrst", userId);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
