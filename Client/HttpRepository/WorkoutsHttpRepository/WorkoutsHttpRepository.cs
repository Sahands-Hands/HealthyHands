using HealthyHands.Shared.Models;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
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
			var responseMessage = await _httpClient.GetAsync("workouts");
			var content = await ValidateContent(responseMessage).ReadAsStringAsync();
			var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
