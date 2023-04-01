// Filename: UserHttpRepositoryTests.cs

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HealthyHands.Client.HttpRepository.UserRepository;
using HealthyHands.Shared.Models;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace HealthyHands.Tests.ClientTests.HttpRepositoryTests
{
    /// <summary>
    /// The UserHttpRepository tests.
    /// </summary>
    public class UserHttpRepositoryTests
    {
        /// <summary>
        /// Tests the GetUserInfo method and expects <see langword="true" />.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_GetUserInfo_ReturnsOk()
        {
            var testUserDto = new UserDto
            {
                Id = "33cf3ff9-27dd-41dd-9667-ecdee83c7b72",
                UserName = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Height = 72,
                Gender = 1,
                ActivityLevel = 1,
                CalorieGoal = 3000,
                BirthDay = DateTime.Parse("2023 - 03 - 21T10: 48:11.8147256"),
                UserMeals = new List<UserMeal>(),
                UserWeights = new List<UserWeight>(),
                UserWorkouts = new List<UserWorkout>()
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255/");
            handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7255/user").ReturnsJsonResponse<UserDto>(HttpStatusCode.OK, testUserDto);
            var userHttpRepository = new UserHttpRepository(client);

            var result = await userHttpRepository.GetUserInfo();

            var obj1Str = JsonSerializer.Serialize(testUserDto); 
            var obj2Str = JsonSerializer.Serialize(result);
            
            Assert.Equal(obj1Str, obj2Str);
        }

        /// <summary>
        /// Tests the UpdateUserInfo method and expects <see langword="true"/>.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_UpdateUserInfo_ReturnsOk()
        {
            var testUserDto = new UserDto
            {
                Id = "33cf3ff9-27dd-41dd-9667-ecdee83c7b72",
                UserName = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Height = 72,
                Gender = 1,
                ActivityLevel = 1,
                CalorieGoal = 3000,
                BirthDay = DateTime.Parse("2023 - 03 - 21T10: 48:11.8147256"),
                UserMeals = new List<UserMeal>(),
                UserWeights = new List<UserWeight>(),
                UserWorkouts = new List<UserWorkout>()
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255/");
            handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7255/user/update").ReturnsResponse(HttpStatusCode.OK);
            var userHttpRepository = new UserHttpRepository(client);

            handlerMock.SetupRequest(HttpMethod.Delete, "https://localhost:7255/user/delete/{userId:string}").ReturnsResponse(HttpStatusCode.OK);

            var result = await userHttpRepository.UpdateUserInfo(testUserDto);

            Assert.True(result);
        }
    }
}
