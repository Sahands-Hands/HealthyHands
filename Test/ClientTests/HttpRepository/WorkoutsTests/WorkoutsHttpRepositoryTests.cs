using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using HealthyHands.Shared.Models;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using HealthyHands.Client.HttpRepository.WorkoutsHttpRepository;
using HealthyHands.Tests.Helpers;

namespace HealthyHands.Tests.ClientTests.HttpRepository.WorkoutsTests
{
	public class WorkoutsHttpRepositoryTests
	{
		# region Property
		private readonly MockHttpHelper _mockHttpHelper = new("https://localhost:7255/workouts");
		# endregion
		
		[Fact]
		public async Task Test_GetWorkouts_ShouldSucceed() 
		{
			// Arrange
			var expectedUserDto = new UserDto
			{
				Id = "b25cbea2-3b57-4400-aef5-52cc035d0fc3",
				UserName = "joesnow",
				FirstName = "Joe",
				LastName = "Snow",
				Height = 72,
				Gender = 1,
				ActivityLevel = 2,
				BirthDay = DateTime.Parse("1999-05-28T21:00:02.497"),
				UserMeals = new List<UserMeal>(),
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>
				{
					new()
					{
						UserWorkoutId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
						WorkoutName = "Cardio",
						WorkoutType = 1,
						Intensity = 2,
						Length = 60,
						WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						CaloriesBurned = 420,
						ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
					}
				}
			};
			var expectedUserDtoJson = JsonSerializer.Serialize(expectedUserDto);

			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Get, 
					RequestUri = ""
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = expectedUserDtoJson
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var workoutsHttpRepository = new WorkoutsHttpRepository(mockClient);

			// Act
			var actualUserDto = await workoutsHttpRepository.GetWorkouts();
			var actualUserDtoJson = JsonSerializer.Serialize(actualUserDto);
			
			//Assert
			Assert.Equal(expectedUserDtoJson, actualUserDtoJson);
		}

		[Fact]
		public async Task Test_GetWorkoutsByDate_ShouldSucceed()
		{
			var expectedUserDto = new UserDto
			{
				Id = "b25cbea2-3b57-4400-aef5-52cc035d0fc3",
				UserName = "joesnow",
				FirstName = "Joe",
				LastName = "Snow",
				Height = 72,
				Gender = 1,
				ActivityLevel = 2,
				BirthDay = DateTime.Parse("1999-05-28T21:00:02.497"),
				UserMeals = new List<UserMeal>(),
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>
				{
					new()
					{
						UserWorkoutId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
						WorkoutName = "Cardio",
						WorkoutType = 1,
						Intensity = 2,
						Length = 60,
						WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						CaloriesBurned = 420,
						ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
					},
				}
			};
			var expectedUserDtoJson = JsonSerializer.Serialize(expectedUserDto);
			var userWorkoutsDateFilter = "2023-03-28T00:00:00.0";
			
			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Get, 
					RequestUri = $"/byDate/{userWorkoutsDateFilter}"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = expectedUserDtoJson
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var workoutsHttpRepository = new WorkoutsHttpRepository(mockClient);

			// Act
			var actualUserDto= await workoutsHttpRepository.GetWorkoutsByDate(userWorkoutsDateFilter);
			var actualUserDtoJson = JsonSerializer.Serialize(actualUserDto);
			
			// Assert
			Assert.Equal(expectedUserDtoJson, actualUserDtoJson);
		}

		[Fact]
		public async Task Test_AddWorkout_ShouldSucceed()
		{ 
			//Arrange
			var workoutToAdd = new UserWorkoutDto
			{
				WorkoutName = "Cardio",
				WorkoutType = 1,
				Intensity = 2,
				Length = 60,
				WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				CaloriesBurned = 420,
				ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
			};

			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Put, 
					RequestUri = "/add"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = ""
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var workoutsHttpRepository = new WorkoutsHttpRepository(mockClient);

			//Act
			var result = await workoutsHttpRepository.AddUserWorkout(workoutToAdd);
			
			// Assert
			Assert.True(result);

		}

		[Fact]
		public async Task Test_UpdateWorkout_ShouldSucceed()
		{
			// Arrange
			var workoutToUpdate = new UserWorkoutDto
			{
				UserWorkoutId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
				WorkoutName = "Cardio",
				WorkoutType = 1,
				Intensity = 2,
				Length = 60,
				WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				CaloriesBurned = 420,
				ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
			};

			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Put, 
					RequestUri = "/update"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = ""
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var workoutsHttpRepository = new WorkoutsHttpRepository(mockClient);

			//Act
			var result = await workoutsHttpRepository.UpdateWorkouts(workoutToUpdate);
			
			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task Test_DeleteWorkout_ShouldSucceed()
		{
			// Arrange
			var workoutIdToDelete = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda109";
			
			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Delete, 
					RequestUri = $"/delete/{workoutIdToDelete}"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = ""
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var workoutsHttpRepository = new WorkoutsHttpRepository(mockClient);

			//Act
			var result = await workoutsHttpRepository.DeleteWorkout(workoutIdToDelete);
			
			// Assert
			Assert.True(result);
		}
	}
}
