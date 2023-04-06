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
        public void UserMeals_WhenInstantiated_IsNotNull()
        {
            // Arrange
            var userDto = new UserDto();

            // Act

            // Assert
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