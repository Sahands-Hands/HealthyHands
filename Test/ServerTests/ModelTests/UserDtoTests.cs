///File Name UserDtoTests.cs

using System;
using System.Collections.Generic;
using Xunit;
using HealthyHands.Shared.Models;

namespace HealthyHands.Tests.ServerTests.ModelTests
{
    public class UserDtoTests
    {
        [Fact]
        public void UserWeights_WhenInstantiated_IsNotNull()
        {
            // Arrange
            var userDto = new UserDto();

            // Act

            // Assert
            Assert.NotNull(userDto.UserWeights);
        }

        [Fact]
        public void UserWeights_AddingUserWeight_IncreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userWeight = new UserWeight { Weight = 180, WeightDate = DateTime.Today };

            // Act
            userDto.UserWeights.Add(userWeight);

            // Assert
            Assert.Equal(1, userDto.UserWeights.Count);
        }

        [Fact]
        public void UserWeights_RemovingUserWeight_DecreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userWeight = new UserWeight { Weight = 180, WeightDate = DateTime.Today };
            userDto.UserWeights.Add(userWeight);

            // Act
            userDto.UserWeights.Remove(userWeight);

            // Assert
            Assert.Equal(0, userDto.UserWeights.Count);
        }
    }
}