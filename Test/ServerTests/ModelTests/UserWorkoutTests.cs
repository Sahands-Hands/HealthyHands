///FIle Name UserWorkoutTests.cs

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
    public class UserWorkoutTests
    {
        [Fact]
        public void UserWorkout_GettersAndSetters_ReturnExpectedValues()
        {
            // Arrange
            var userWorkout = new UserWorkout();
            userWorkout.UserWorkoutId = "1";
            userWorkout.WorkoutName = "Jog";
            userWorkout.WorkoutType = 2;
            userWorkout.Intensity = 3;
            userWorkout.Length = 60;
            userWorkout.WorkoutDate = DateTime.Now.Date;
            userWorkout.CaloriesBurned = 600;
            userWorkout.ApplicationUserId = "user123";

            // Act
            var userWorkoutId = userWorkout.UserWorkoutId;
            var workoutName = userWorkout.WorkoutName;
            var workoutType = userWorkout.WorkoutType;
            var intensity = userWorkout.Intensity;
            var length = userWorkout.Length;
            var workoutDate = userWorkout.WorkoutDate;
            var caloriesBurned = userWorkout.CaloriesBurned;
            var applicationUserId = userWorkout.ApplicationUserId;

            // Assert
            Assert.Equal("1", userWorkoutId);
            Assert.Equal("Jog", workoutName);
            Assert.Equal(2, workoutType);
            Assert.Equal(3, intensity);
            Assert.Equal(60, length);
            Assert.Equal(DateTime.Now.Date, workoutDate);
            Assert.Equal(600, caloriesBurned);
            Assert.Equal("user123", applicationUserId);
        }
    }
}