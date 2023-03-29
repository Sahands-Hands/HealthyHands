using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HealthyHands.Client.HttpRepository.WeightHttpRepository;
using HealthyHands.Shared.Models;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Flurl.Http.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;

namespace HealthyHands.Tests
{
    public class WeightHttpRepositoryTests
    {
        [Fact]

        public async Task GetWeights_ReturnsExpectedResult()
        { // Arrange

            var expectedResult = new UserDto
            {
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
            httpClient.BaseAddress = new Uri("https://localhost:44300");
            var weightHttpRepository = new WeightHttpRepository(httpClient);
            // Act
            var result = await weightHttpRepository.GetWeights();
            // Assert
            Assert.Equal(expectedResult.UserWeights[0].UserWeightId, result.UserWeights[0].UserWeightId);
            Assert.Equal(expectedResult.UserWeights[0].Weight, result.UserWeights[0].Weight);
            Assert.Equal(expectedResult.UserWeights[0].WeightDate, result.UserWeights[0].WeightDate);
            Assert.Equal(expectedResult.UserWeights[0].ApplicationUserId, result.UserWeights[0].ApplicationUserId);


        }

        }
    }





 

