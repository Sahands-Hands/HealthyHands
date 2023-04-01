using HealthyHands.Shared.Models;
using System.Net.Http.Json;
using HealthyHands.Client.HttpRepository.WorkoutsRepository;

namespace HealthyHands.Client.HttpRepository.WorkoutsHttpRepository
{
    public class WorkoutsHttpRepository : IWorkoutsHttpRepository
    {
        private readonly HttpClient _httpClient;
        
        public WorkoutsHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
		public async Task<UserDto> GetWorkouts()
		{
			var user = await _httpClient.GetFromJsonAsync<UserDto>("workouts");
			return user;
		}
		
		public async Task<UserDto> GetWorkoutsByDate(string date)
		{
			var user = await _httpClient.GetFromJsonAsync<UserDto>($"workouts/byDate/{date}");
			return user;
		}
		
		public async Task<bool> AddUserWorkout(UserWorkoutDto userWorkoutDto)
		{		
			var response = await _httpClient.PutAsJsonAsync("workouts/add", userWorkoutDto);
			return response.IsSuccessStatusCode;
		}
		
		public async Task<bool> UpdateWorkouts(UserWorkoutDto userWorkoutDto)
		{
			var response = await _httpClient.PutAsJsonAsync("workouts/update", userWorkoutDto);
			return response.IsSuccessStatusCode;
		}
		
		public async Task<bool> DeleteWorkout(string userWorkoutId)
		{
			var response = await _httpClient.DeleteAsync($"workouts/delete/{userWorkoutId}");
			return response.IsSuccessStatusCode;
		}
    }
}
