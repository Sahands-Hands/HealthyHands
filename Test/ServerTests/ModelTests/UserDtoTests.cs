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
        public void UserMeals_WhenInstantiated_IsNotNull()
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
            Assert.NotNull(userDto.UserMeals);
        }

        [Fact]
        public void UserWorkouts_AddingUserWorkout_IncreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userWorkout = new UserWorkout {
                UserWorkoutId = "1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = DateTime.Now.Date,
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };

            // Act
            userDto.UserWorkouts.Add(userWorkout);

            // Assert
            Assert.Equal(1, userDto.UserWorkouts.Count);
        }

        [Fact]
        public void UserWorkouts_RemovingUserWorkout_DecreasesCount()
        {
            // Arrange
            var userDto = new UserDto();
            var userWorkout = new UserWorkout {
                UserWorkoutId = "1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = DateTime.Now.Date,
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };
            userDto.UserWorkouts.Add(userWorkout);

            // Act
            userDto.UserWorkouts.Remove(userWorkout);

            // Assert
            Assert.Equal(0, userDto.UserWorkouts.Count);
        }
    }
}