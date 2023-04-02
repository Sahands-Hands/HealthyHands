using System;
using System.Collections.Generic;
using Xunit;
using HealthyHands.Shared.Models;

namespace HealthyHands.Tests.ServerTests.ModelTests
{
    public class UserDtoTests
    {
        [Fact]
        public void UserMeals_WhenInstantiated_IsNotNull()
        {
            // Arrange
            var userDto = new UserDto();

            // Act

            // Assert
            Assert.NotNull(userDto.UserMeals);
        }

        [Fact]
        public void UserMeals_AddingUserMeal_IncreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userMeal = new UserMeal { MealName = "Breakfast", MealDate = DateTime.Today };

            // Act
            userDto.UserMeals.Add(userMeal);

            // Assert
            Assert.Equal(1, userDto.UserMeals.Count);
        }

        [Fact]
        public void UserMeals_RemovingUserMeal_DecreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userMeal = new UserMeal { MealName = "Lunch", MealDate = DateTime.Today };
            userDto.UserMeals.Add(userMeal);

            // Act
            userDto.UserMeals.Remove(userMeal);

            // Assert
            Assert.Equal(0, userDto.UserMeals.Count);
        }
    }
}