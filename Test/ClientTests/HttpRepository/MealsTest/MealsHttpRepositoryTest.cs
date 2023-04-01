using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using HealthyHands.Shared.Models;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using HealthyHands.Client.HttpRepository.MealHttpRepository;
using HealthyHands.Tests.Helpers;

namespace HealthyHands.Tests.ClientTests.HttpRepository.MealsTests
{
	public class MealsHttpRepositoryTests
	{
		#region Property
		private readonly MockHttpHelper _mockHttpHelper = new("https://localhost:7255/meals");
		#endregion

		[Fact]
		public async Task Test_GetMeals_ShouldSucceed()
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
				UserMeals = new List<UserMeal>
                {
                    new()
                    {
						UserMealId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
						MealName = "Gyros",
						MealDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						Calories = 60,
						Protein = 40,
						Carbs = 20,
						Fat = 15,
						Sugar = 38,
						ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
					}
                },
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>()
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
			var workoutsHttpRepository = new MealsHttpRepository(mockClient);

			// Act
			var actualUserDto = await workoutsHttpRepository.GetMeals();
			var actualUserDtoJson = JsonSerializer.Serialize(actualUserDto);

			//Assert
			Assert.Equal(expectedUserDtoJson, actualUserDtoJson);
		}

		[Fact]
		public async Task Test_GetMealsByDate_ShouldSucceed()
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
				UserMeals = new List<UserMeal>
                {
                    new()
                    {
						UserMealId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
						MealName = "Gyros",
						MealDate = DateTime.Parse("2023-03-28T21:00:02.497"),
						Calories = 60,
						Protein = 40,
						Carbs = 20,
						Fat = 15,
						Sugar = 38,
						ApplicationUserId = "b25cbea2-3b57-4400-aef5-52cc035d0fc3"
					}
                },
				UserWeights = new List<UserWeight>(),
				UserWorkouts = new List<UserWorkout>()
			};
			var expectedUserDtoJson = JsonSerializer.Serialize(expectedUserDto);
			var userMealsDateFilter = "2023-03-28T00:00:00.0";

			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Get,
					RequestUri = $"/byDate/{userMealsDateFilter}"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = expectedUserDtoJson
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var mealsHttpRepository = new MealsHttpRepository(mockClient);

			// Act
			var actualUserDto = await mealsHttpRepository.GetMealsByDate(userMealsDateFilter);
			var actualUserDtoJson = JsonSerializer.Serialize(actualUserDto);

			// Assert
			Assert.Equal(expectedUserDtoJson, actualUserDtoJson);
		}

		[Fact]
		public async Task Test_AddMeal_ShouldSucceed()
		{
			//Arrange
			var mealToAdd = new UserMealDto
			{
				UserMealId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
				MealName = "Gyros",
				MealDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				Calories = 60,
				Protein = 40,
				Carbs = 20,
				Fat = 15,
				Sugar = 38,
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
			var mealsHttpRepository = new MealsHttpRepository(mockClient);

			//Act
			var result = await mealsHttpRepository.AddMeals(mealToAdd);

			// Assert
			Assert.True(result);

		}

		[Fact]
		public async Task Test_UpdateMeal_ShouldSucceed()
		{
			// Arrange
			var mealToUpdate = new UserMealDto
			{
				UserMealId = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda10",
				MealName = "Gyros",
				MealDate = DateTime.Parse("2023-03-28T21:00:02.497"),
				Calories = 60,
				Protein = 40,
				Carbs = 20,
				Fat = 15,
				Sugar = 38,
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
			var mealsHttpRepository = new MealsHttpRepository(mockClient);

			//Act
			var result = await mealsHttpRepository.UpdateMeals(mealToUpdate);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task Test_DeleteMeal_ShouldSucceed()
		{
			// Arrange
			var mealIdToDelete = "e4a578e7-ba20-4a88-a0a2-4a2f18fdda109";

			var mockHttpMessageHandler = _mockHttpHelper.CreateMessageHandler(
				new HttpRequest
				{
					Method = HttpMethod.Delete,
					RequestUri = $"/delete/{mealIdToDelete}"
				},
				new HttpResponse
				{
					StatusCode = HttpStatusCode.OK,
					Response = ""
				});
			var mockClient = _mockHttpHelper.CreateClient(mockHttpMessageHandler);
			var mealsHttpRepository = new MealsHttpRepository(mockClient);

			//Act
			var result = await mealsHttpRepository.DeleteMeals(mealIdToDelete);

			// Assert
			Assert.True(result);
		}
	}
}
