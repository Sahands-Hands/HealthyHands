using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using HealthyHands.Shared.Models;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using HealthyHands.Client.HttpRepository.WorkoutsRepository;
using Moq;
using Moq.Contrib.HttpClient;

namespace HealthyHands.Tests.ClientTests.HttpRepository.WorkoutsTests
{
	public class WorkoutsHttpRepositoryTests
	{
		[Fact]
		public async Task Test_GetWorkouts_ShouldSucceed() 
		{
			// Arrange
			var userWorkoutsResponse = new UserDto
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
					new()
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
			var userWorkoutsResponseJson = JsonSerializer.Serialize(userWorkoutsResponse);
			
			var handlerMock = new Mock<HttpMessageHandler>();
			var clientMock = handlerMock.CreateClient();
			clientMock.BaseAddress = new Uri("https://localhost:7255");
			handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7255/workouts")
				.ReturnsResponse(HttpStatusCode.OK, userWorkoutsResponseJson);

			var workoutsHttpRepository = new WorkoutsHttpRepository(clientMock);

			// Act
			var returnedUserDto = await workoutsHttpRepository.GetWorkouts();
			var obj2 = JsonSerializer.Serialize(returnedUserDto);
			
			//Assert
			Assert.Equal(userWorkoutsResponseJson, obj2);
		}

		[Fact]
		public async Task Test_GetWorkoutsByDate_ShouldSucceed()
		{
			var userDtoResponseExpected = new UserDto
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
					new()
					{
						UserWorkoutId = "1746fa13-f1b0-4ee5-a44b-d38be3037299",
						WorkoutName = "Cardio",
						WorkoutType = 1,
						Intensity = 2,
						Length = 30,
						WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						CaloriesBurned = 420,
						ApplicationUserId = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd"
					},
				}
			};
			var userDtoResponseExpectedJson = JsonSerializer.Serialize(userDtoResponseExpected);
			var userWorkoutsDateFilter = new DateTime(2023, 3, 28).Date.ToString(CultureInfo.InvariantCulture);
			
			var handlerMock = new Mock<HttpMessageHandler>();
			var clientMock = handlerMock.CreateClient();
			clientMock.BaseAddress = new Uri("https://localhost:7255");
			handlerMock.SetupRequest(HttpMethod.Get, $"https://localhost:7255/workouts/byDate/{userWorkoutsDateFilter}")
				.ReturnsResponse(HttpStatusCode.OK, userDtoResponseExpectedJson);

			var workoutsHttpRepository = new WorkoutsHttpRepository(clientMock);

			// Act
			var userDtoResponseReceived = await workoutsHttpRepository.GetWorkoutsByDate(userWorkoutsDateFilter);
			var userDtoResponseReceivedJson = JsonSerializer.Serialize(userDtoResponseReceived);
			
			Assert.Equal(userDtoResponseExpectedJson, userDtoResponseReceivedJson);
		}

		[Fact]
		public async Task Test_AddWorkout_ShouldSucceed()
		{ 
			//Arrange
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

			var handlerMock = new Mock<HttpMessageHandler>();
			var clientMock = handlerMock.CreateClient();
			clientMock.BaseAddress = new Uri("https://localhost:7255");
			handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7255/workouts/add")
				.ReturnsResponse(HttpStatusCode.OK);

			var workoutsHttpRepository = new WorkoutsHttpRepository(clientMock);

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
				UserWorkoutId = "1746fa13-f1b0-4ee5-a44b-d38be3037299",
				WorkoutName = "Cardio",
				WorkoutType = 1,
				Intensity = 2,
				Length = 30,
				WorkoutDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				CaloriesBurned = 420,
				ApplicationUserId = "29e6d8b3-2275-4acd-a29a-6fe27593d2fd"
			};
			var workoutToUpdateJson = JsonSerializer.Serialize(workoutToUpdate);

			var handlerMock = new Mock<HttpMessageHandler>();
			var clientMock = handlerMock.CreateClient();
			clientMock.BaseAddress = new Uri("https://localhost:7255");
			handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7255/workouts/update")
				.ReturnsResponse(HttpStatusCode.OK);

			var workoutsHttpRepository = new WorkoutsHttpRepository(clientMock);

			//Act
			var result = await workoutsHttpRepository.UpdateWorkouts(workoutToUpdate);
			
			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task Test_DeleteWorkout_ShouldSucceed()
		{
			// Arrange
			string workoutIdToDelete = "1746fa13-f1b0-4ee5-a44b-d38be3037299";

			var handlerMock = new Mock<HttpMessageHandler>();
			var clientMock = handlerMock.CreateClient();
			clientMock.BaseAddress = new Uri("https://localhost:7255");
			handlerMock.SetupRequest(HttpMethod.Delete, $"https://localhost:7255/workouts/delete/{workoutIdToDelete}")
				.ReturnsResponse(HttpStatusCode.OK);

			var workoutsHttpRepository = new WorkoutsHttpRepository(clientMock);

			//Act
			var result = await workoutsHttpRepository.DeleteWorkout(workoutIdToDelete);
			
			// Assert
			Assert.True(result);
		}
	}
}
