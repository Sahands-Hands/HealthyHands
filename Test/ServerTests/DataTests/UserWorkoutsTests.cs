///File name UserWorkoutsTests.cs

using HealthyHands.Server.Data;
using HealthyHands.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Duende.IdentityServer.EntityFramework.Options;

namespace HealthyHands.Tests.DataTests
{
    public class UserWorkoutsTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public UserWorkoutsTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
        }

        [Fact]
        public async Task TestUserWorkoutsCRUD()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options, _operationalStoreOptions);
            var userWorkout = new UserWorkout
            {
                UserWorkoutId = "1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = DateTime.Now.Date,
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };

            // Act: Add UserMeal
            context.UserWorkouts.Add(userWorkout);
            await context.SaveChangesAsync();

            // Assert: UserMeal is added
            var addedUserWorkout = await context.UserWorkouts.FindAsync(userWorkout.UserWorkoutId);
            Assert.NotNull(addedUserWorkout);
            Assert.Equal(userWorkout.WorkoutName, addedUserWorkout.WorkoutName);

            // Act: Update UserMeal
            addedUserWorkout.WorkoutName = "Updated Test Workout";
            context.UserWorkouts.Update(addedUserWorkout);
            await context.SaveChangesAsync();

            // Assert: UserMeal is updated
            var updatedUserWorkout = await context.UserWorkouts.FindAsync(userWorkout.UserWorkoutId);
            Assert.NotNull(updatedUserWorkout);
            Assert.Equal("Updated Test Workout", updatedUserWorkout.WorkoutName);

            // Act: Delete UserMeal
            context.UserWorkouts.Remove(updatedUserWorkout);
            await context.SaveChangesAsync();

            // Assert: UserMeal is deleted
            var deletedUserWorkout = await context.UserWorkouts.FindAsync(userWorkout.UserWorkoutId);
            Assert.Null(deletedUserWorkout);
        }
    }
}