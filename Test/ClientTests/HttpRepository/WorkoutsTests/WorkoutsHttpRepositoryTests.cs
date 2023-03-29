using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using HealthyHands.Shared.Models;
using System.Text.Json;
using System.Net;
using System.Net.Http;

namespace HealthyHands.Tests.ClientTests.HttpRepository.WorkoutsTests
{
	public class WorkoutsHttpRepositoryTests
	{
		[Fact]
		public async Task Test_GetWorkouts() 
		{
			var mockHttp = new MockHttpMessageHandler();

			var testWorkoutResponse = new UserDto
			{
				Id = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd",
				UserName = "",
				FirstName = null,
				LastName = null,
				Height = null,
				Gender = null,
				ActivityLevel = null,
				BirthDay = null,
				UserMeals = new List<UserMeal>(),
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>
				{
					new UserWorkout
					{
						UserWorkoutId = "1746fa13-f1b0-4ee5-a44b-d38be3037299",
						WorkoutName = "Cardio",
						WorkoutType = 1,
						Intensity = 2,
						Length = 30,
						WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						CaloriesBurned = 420,
						ApplicationUserId = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd"
					}
				}
			};
			string testWorkoutResponseJson = JsonSerializer.Serialize(testWorkoutResponse);
			mockHttp.When("https://localhost:7255/workouts")
				.Respond("application/json", testWorkoutResponseJson);

			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri("https://localhost:7255/");
			var WorkoutsHttpRepository = new Client.HttpRepository.WorkoutsRepository.WorkoutsHttpRepository(client);

			var workout = await WorkoutsHttpRepository.GetWorkouts();
			mockHttp.VerifyNoOutstandingExpectation();

			Assert.Equal(expected: testWorkoutResponse.Id, actual: workout.Id);
			Assert.Equal(expected: testWorkoutResponse.UserName, actual: workout.UserName);
			Assert.Equal(expected: testWorkoutResponse.FirstName, actual: workout.FirstName);
			Assert.Equal(expected: testWorkoutResponse.LastName, actual: workout.LastName);
			Assert.Equal(expected: testWorkoutResponse.Height, actual: workout.Height);
			Assert.Equal(expected: testWorkoutResponse.Gender, actual: workout.Gender);
			Assert.Equal(expected: testWorkoutResponse.ActivityLevel, actual: workout.ActivityLevel);
			Assert.Equal(expected: testWorkoutResponse.BirthDay, actual: workout.BirthDay);
			Assert.Equal(expected: testWorkoutResponse.UserMeals, actual: workout.UserMeals);
			Assert.Equal(expected: testWorkoutResponse.UserWeights, actual: workout.UserWeights);
			Assert.Equal(expected: testWorkoutResponse.UserWorkouts.Count, actual: workout.UserWorkouts.Count);

			int i = 0;
			foreach(var item in workout.UserWorkouts)
			{ 
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].UserWorkoutId, actual: item.UserWorkoutId);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutName, actual: item.WorkoutName);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutType, actual: item.WorkoutType);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].Intensity, actual: item.Intensity);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].Length, actual: item.Length);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutDate, actual: item.WorkoutDate);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].CaloriesBurned, actual: item.CaloriesBurned);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].ApplicationUserId, actual: item.ApplicationUserId);
				i++;
			}

        }

		[Fact]
		public async Task Test_GetWorkoutsByDate()
		{
			var mockHttp = new MockHttpMessageHandler();

			var testWorkoutResponse = new UserDto
			{
				Id = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd",
				UserName = "",
				FirstName = null,
				LastName = null,
				Height = null,
				Gender = null,
				ActivityLevel = null,
				BirthDay = null,
				UserMeals = new List<UserMeal>(),
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>
				{
					new UserWorkout
					{
						UserWorkoutId = "1746fa13-f1b0-4ee5-a44b-d38be3037299",
						WorkoutName = "Cardio",
						WorkoutType = 1,
						Intensity = 2,
						Length = 30,
						WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						CaloriesBurned = 420,
						ApplicationUserId = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd"
					}
				}
			};
			string testWorkoutResponseJson = JsonSerializer.Serialize(testWorkoutResponse);
			mockHttp.When("https://localhost:7255/workouts/byDate/{date:string}")
				.Respond("application/json", testWorkoutResponseJson);

			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri("https://localhost:7255/");
			var WorkoutsHttpRepository = new Client.HttpRepository.WorkoutsRepository.WorkoutsHttpRepository(client);
			string date = "2023-03-28T21:00:02.497";
			var workout = await WorkoutsHttpRepository.GetWorkoutsByDate(date);
			mockHttp.VerifyNoOutstandingExpectation();

			Assert.Equal(expected: testWorkoutResponse.Id, actual: workout.Id);
			Assert.Equal(expected: testWorkoutResponse.UserName, actual: workout.UserName);
			Assert.Equal(expected: testWorkoutResponse.FirstName, actual: workout.FirstName);
			Assert.Equal(expected: testWorkoutResponse.LastName, actual: workout.LastName);
			Assert.Equal(expected: testWorkoutResponse.Height, actual: workout.Height);
			Assert.Equal(expected: testWorkoutResponse.Gender, actual: workout.Gender);
			Assert.Equal(expected: testWorkoutResponse.ActivityLevel, actual: workout.ActivityLevel);
			Assert.Equal(expected: testWorkoutResponse.BirthDay, actual: workout.BirthDay);
			Assert.Equal(expected: testWorkoutResponse.UserMeals, actual: workout.UserMeals);
			Assert.Equal(expected: testWorkoutResponse.UserWeights, actual: workout.UserWeights);
			Assert.Equal(expected: testWorkoutResponse.UserWorkouts.Count, actual: workout.UserWorkouts.Count);

			int i = 0;
			foreach (var item in workout.UserWorkouts)
			{
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].UserWorkoutId, actual: item.UserWorkoutId);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutName, actual: item.WorkoutName);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutType, actual: item.WorkoutType);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].Intensity, actual: item.Intensity);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].Length, actual: item.Length);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].WorkoutDate, actual: item.WorkoutDate);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].CaloriesBurned, actual: item.CaloriesBurned);
				Assert.Equal(expected: testWorkoutResponse.UserWorkouts[i].ApplicationUserId, actual: item.ApplicationUserId);
				i++;
			}

		}

		[Fact]
		public async Task Test_AddUserWorkout()
		{ 
			//Arrange
			var mockHttp = new MockHttpMessageHandler();
			var workoutToAdd = new UserWorkoutDto
			{
				UserWorkoutId = "",
				WorkoutName = "Lifting",
				WorkoutType = 1,
				Intensity = 2,
				Length = 500,
				WorkoutDate = DateTime.Parse("2023-03-29T21:00:02.497"),
				CaloriesBurned = 69,
				ApplicationUserId = ""
			};

			var responseJson = @"{success"":true}";
			string testWorkoutResponseJson = JsonSerializer.Serialize(workoutToAdd);
			mockHttp.When(HttpMethod.Put, "https://localhost:7255/workouts/add")
				.WithContent(testWorkoutResponseJson)
				.Respond("application/json", responseJson);

			string testByDate = JsonSerializer.Serialize(workoutToAdd);
			mockHttp.When("https://localhost:7255/workouts/byDate/{date:string}")
				.Respond("application/json", testByDate);

			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri("https://localhost:7255/");
			var workoutsHttpRepository = new Client.HttpRepository.WorkoutsRepository.WorkoutsHttpRepository(client);

			//Act
			await workoutsHttpRepository.AddUserWorkout(workoutToAdd);

			string date = "2023-03-29T21:00:02.497";
			var workout = await workoutsHttpRepository.GetWorkoutsByDate(date);

			int i = 0;
			foreach (var item in workout.UserWorkouts)
			{
				Assert.Equal(expected: workoutToAdd.UserWorkoutId, actual: item.UserWorkoutId);
				Assert.Equal(expected: workoutToAdd.WorkoutName, actual: item.WorkoutName);
				Assert.Equal(expected: workoutToAdd.WorkoutType, actual: item.WorkoutType);
				Assert.Equal(expected: workoutToAdd.Intensity, actual: item.Intensity);
				Assert.Equal(expected: workoutToAdd.Length, actual: item.Length);
				Assert.Equal(expected: workoutToAdd.WorkoutDate, actual: item.WorkoutDate);
				Assert.Equal(expected: workoutToAdd.CaloriesBurned, actual: item.CaloriesBurned);
				Assert.Equal(expected: workoutToAdd.ApplicationUserId, actual: item.ApplicationUserId);
				
			}
			mockHttp.VerifyNoOutstandingExpectation();

		}


		[Fact]
		public async Task Test_UpdateWorkouts()
		{
			
			var workoutToUpdate = new UserWorkoutDto
			{
				UserWorkoutId = "1746fa13-f1b0-4ee5-a44b-d38be3037299",
				WorkoutName = "Cardio",
				WorkoutType = 1,
				Intensity = 2,
				Length = 30,
				WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				CaloriesBurned = 420,
				ApplicationUserId = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd"
			};
			string testWorkoutsDtoJson = JsonSerializer.Serialize(workoutToUpdate);

			var mockHttpMessageHandler = new MockHttpMessageHandler();
			var request = mockHttpMessageHandler.When(HttpMethod.Put, "https://localhost:7255/workouts/update")
				.WithContent(testWorkoutsDtoJson)
				.Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

			var client = mockHttpMessageHandler.ToHttpClient();
			client.BaseAddress = new Uri("https://localhost:7255/");
			var workoutsHttpRepository = new Client.HttpRepository.WorkoutsRepository.WorkoutsHttpRepository(client);

			
			bool result = await workoutsHttpRepository.UpdateWorkouts(workoutToUpdate);

			mockHttpMessageHandler.VerifyNoOutstandingExpectation();
			//Assert.Equal(expected: 1, actual: mockHttpMessageHandler.GetMatchCount(request));
			Assert.True(result);
			
		}
	

	}

}
