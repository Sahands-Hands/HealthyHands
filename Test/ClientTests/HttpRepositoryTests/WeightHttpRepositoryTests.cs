using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace HealthyHands.Tests.ClientTests.HttpRepositoryTests
{
    public class WeightHttpRepositoryTests
    {
        [Fact]
        public async Task GetWeights_ReturnsExpectedResult()
        { // Arrange

            var testUserDto = new UserDto
            {
                Id = "",
                UserWeights = new List<UserWeight>
    {
        new UserWeight
        {
            UserWeightId = "1",
            Weight = 70.5,
            WeightDate = new DateTime(2022, 1, 1),
            ApplicationUserId = "123"
        },

    }
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255/");
            handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7255/weights").ReturnsJsonResponse<UserDto>(HttpStatusCode.OK, testUserDto);

            var weightHttpRepository = new WeightHttpRepository(client);

            // Act
            var result = await weightHttpRepository.GetWeights();
            // Assert

            var obj1Str = JsonSerializer.Serialize(testUserDto);
            var obj2Str = JsonSerializer.Serialize(result);
            Assert.Equal(obj1Str, obj2Str);

        }

        [Fact]
        public async Task GetWeightsByWeightDate_ReturnsExpectedResult()
        {
            // Arrange
            var testUserDto = new UserDto
            {
                Id = "",
                UserWeights = new List<UserWeight>
                    {

                        new UserWeight
                        {
                            UserWeightId = "1",
                            Weight = 70.5,
                            WeightDate = new DateTime(2022, 1, 1),
                            ApplicationUserId = "123"
                        },
                    }
            };


            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255");
            handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7255/weights/byDate/{date:string}").ReturnsJsonResponse<UserDto>(HttpStatusCode.OK, testUserDto);

            var weightHttpRepository = new WeightHttpRepository(client);


            // Act
            var result = await weightHttpRepository.GetWeightsByWeightDate();

            // Assert
            var obj1Str = JsonSerializer.Serialize(testUserDto);
            var obj2Str = JsonSerializer.Serialize(result);
            Assert.Equal(obj1Str, obj2Str);
        }
        [Fact]
        public async Task AddWeight_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var testUserDto = new UserWeightDto
            {
                Weight = 70.5,
                WeightDate = new DateTime(2022, 1, 1),
                ApplicationUserId = "123"
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255");
            handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7255/weights/add").ReturnsJsonResponse(HttpStatusCode.OK);
            var weightHttpRepository = new WeightHttpRepository(client);

            // Act
            var result = await weightHttpRepository.AddWeight(testUserDto);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateWeight_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var testUserDto = new UserWeightDto
            {
                Weight = 70.5,
                WeightDate = new DateTime(2022, 1, 1),
                ApplicationUserId = "123"

            };
            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255");
            handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7255/weights/update").ReturnsJsonResponse(HttpStatusCode.OK);
            var weightHttpRepository = new WeightHttpRepository(client);

            // Act
            var result = await weightHttpRepository.UpdateWeight(testUserDto);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteWeight_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            string userWeightId = "someID";
            var handlerMock = new Mock<HttpMessageHandler>();
            var client = handlerMock.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7255");
            handlerMock.SetupRequest(HttpMethod.Delete, $"https://localhost:7255/weights/delete/{userWeightId}").ReturnsJsonResponse(HttpStatusCode.OK);
            var weightHttpRepository = new WeightHttpRepository(client);
            // Act
            var result = await weightHttpRepository.DeleteWeight(userWeightId);
            // Assert
            Assert.True(result);
        }
    }
}







