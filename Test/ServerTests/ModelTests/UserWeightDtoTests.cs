///FIle Name UserWeightDtoTests.cs

using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.MealsRepository;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using HealthyHands.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HealthyHands.Tests.ServerTests.ModelTests
{
    public class UserWeightDtoTests
    {
        [Fact]
        public void UserWeightDto_GettersAndSetters_ReturnExpectedValues()
        {
            // Arrange
            var userWeight = new UserWeightDto();
            userWeight.UserWeightId = "1";
            userWeight.Weight = 180;
            userWeight.WeightDate = DateTime.Now.Date;
            userWeight.ApplicationUserId = "user123";

            // Act
            var userWeightId = userWeight.UserWeightId;
            var weight = userWeight.Weight;
            var weightDate = userWeight.WeightDate;
            var applicationUserId = userWeight.ApplicationUserId;

            // Assert
            Assert.Equal("1", userWeightId);
            Assert.Equal(180, weight);
            Assert.Equal(DateTime.Now.Date, weightDate);
            Assert.Equal("user123", applicationUserId);
        }
    }
}