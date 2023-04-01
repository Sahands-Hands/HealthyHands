using HealthyHands.Shared.Models;
using System.Net.Http.Json;
namespace HealthyHands.Client.HttpRepository.MealHttpRepository
{
    public class MealsHttpRepository : IMealsHttpRepository
    {
        public readonly HttpClient _httpClient;
        public MealsHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// This method uses HTTP client to send a GET request to the API
        /// </summary>
        /// <returns>meals</returns>
        public async Task<UserDto> GetMeals()
        {
            UserDto meals = await _httpClient.GetFromJsonAsync<UserDto>("meals");
            return meals;
        }
        /// <summary>
        /// This method uses HTTP client to send a PUT request to the API and update UserMeals
        /// </summary>
        /// <param name="userMealDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateMeals(UserMealDto userMealDto)
        {
            var response = await _httpClient.PutAsJsonAsync("meals/update", userMealDto);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// This method uses HTTP client GET request that will get user meals based on the date entered
        /// </summary>
        /// <param name="date"></param>
        /// <returns>meals</returns>
        public async Task<UserDto> GetMealsByDate(string date)
        {
            UserDto meals = await _httpClient.GetFromJsonAsync<UserDto>("meals/byDate/{date:string}");
            return meals;
        }
        /// <summary>
        /// This method uses HTTP client DELETE request that will delete a user meal  based on meal id
        /// </summary>
        /// <param name="userMealId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMeals(string userMealId)
        {
            var response = await _httpClient.DeleteAsync($"meals/delete/{userMealId}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> AddMeals(UserMealDto userMealDto)
        {
            var response = await _httpClient.PostAsJsonAsync("meals/add", userMealDto);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
