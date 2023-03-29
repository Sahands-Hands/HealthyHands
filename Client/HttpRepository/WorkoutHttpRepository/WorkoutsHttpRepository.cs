using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using System.Security.Claims;


namespace HealthyHands.Client.HttpRepository.WorkoutsRepository
{
    public class WorkoutsHttpRepository : IWorkoutsHttpRepository
    {
        public readonly HttpClient _httpClient;
        public WorkoutsHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
		public async Task<UserDto> GetWorkouts()
		{
			UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("workouts");
			return user;
		}


		public async Task<UserDto> GetWorkoutsByDate(string date)
		{
			UserDto user = await _httpClient.GetFromJsonAsync<UserDto>("workouts/byDate/{date:string}");
			return user;
		}

		public async Task<bool> UpdateWorkouts(UserWorkoutDto userWorkoutDto)
		{
			var response = await _httpClient.PutAsJsonAsync("workouts/update", userWorkoutDto);
			if(!response.IsSuccessStatusCode)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteWorkout(string userWorkoutId)
		{
			var response = await _httpClient.DeleteAsync("delete/{int:userWorkoutId}");
			if(!response.IsSuccessStatusCode)
			{
				return false;
			}
			return true;
		}
		public async Task<bool> AddUserWorkout(UserWorkoutDto userWorkoutDto)
		{		
			var response = await _httpClient.PutAsJsonAsync("workouts/add", userWorkoutDto);
			if(!response.IsSuccessStatusCode)
			{
				return false;
			}
			return true;
		}


	}
}
