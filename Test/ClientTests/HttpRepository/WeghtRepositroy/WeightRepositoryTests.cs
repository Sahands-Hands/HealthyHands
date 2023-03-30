using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthyHands.Tests
{
    public class WeightHttpRepositoryTests
    {
        [Fact]

        public async Task GetWeights_ReturnsExpectedResult()
        { // Arrange

            var expectedResult = new UserDto
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
        // add more UserWeight objects here if necessary
    }
            };

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedResult))
                });
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7255");
            var weightHttpRepository = new WeightHttpRepository(httpClient);
            // Act
            var result = await weightHttpRepository.GetWeights();
            // Assert

            var obj1Str = JsonSerializer.Serialize(expectedResult);
            var obj2Str = JsonSerializer.Serialize(result);

            Assert.Equal(obj1Str, obj2Str);




        }

        [Fact]
        public async Task GetWeightsByWeightDate_ReturnsExpectedResult()
        {
            // Arrange
            var expectedResult = new UserDto
            {
                // add properties and values here to represent the expected result

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

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedResult))
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(" https://localhost:7255");
            var weightHttpRepository = new WeightHttpRepository(httpClient);

            // Act
            var result = await weightHttpRepository.GetWeightsByWeightDate();

            // Assert
            var obj1Str = JsonSerializer.Serialize(expectedResult);
            var obj2Str = JsonSerializer.Serialize(result);

            Assert.Equal(obj1Str, obj2Str);


        }
        [Fact]
        public async Task AddWeight_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var userWeightDto = new UserWeightDto
            {
                // add properties and values here to represent the weight data being added

                Weight = 70.5,
                WeightDate = new DateTime(2022, 1, 1),
                ApplicationUserId = "123"




            };

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(" https://localhost:7255");
            var weightHttpRepository = new WeightHttpRepository(httpClient);

            // Act
            var result = await weightHttpRepository.AddWeight(userWeightDto);

            // Assert
            Assert.True(result);
        }
    }
}







