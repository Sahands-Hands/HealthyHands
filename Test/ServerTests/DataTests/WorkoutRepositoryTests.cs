///File Name WorkoutRepositoryTests.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WorkoutsRepository;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;

namespace HealthyHands.Tests.WorkoutsRepositoryTests
{
    public class WorkoutRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkoutsRepository _repository;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public WorkoutRepositoryTests()
        {
            // Set up a new ApplicationDbContext and WorkoutsRepository for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(options, operationalStoreOptions);
            _repository = new WorkoutsRepository(_context);
        }

        [Fact]
        public async Task GetUserDtoWithAllWorkouts_ReturnsCorrectUserDto()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var workout1 = new UserWorkout {
                UserWorkoutId = "Workout 1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020,3,20),
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };
            var workout2 = new UserWorkout {
                UserWorkoutId = "Workout 2",
                WorkoutName = "Weights",
                WorkoutType = 5,
                Intensity = 2,
                Length = 75,
                WorkoutDate = new DateTime(2020,3,20),
                CaloriesBurned = 500,
                ApplicationUserId = "user123"
            };
            user.UserWorkouts.Add(workout1);
            user.UserWorkouts.Add(workout2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = await _repository.GetUserDtoWithAllWorkouts(user.Id);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(2, userDto.UserWorkouts.Count);
            Assert.Equal(workout1.UserWorkoutId, userDto.UserWorkouts.ElementAt(0).UserWorkoutId);
            Assert.Equal(workout1.WorkoutName, userDto.UserWorkouts.ElementAt(0).WorkoutName);
            Assert.Equal(workout2.UserWorkoutId, userDto.UserWorkouts.ElementAt(1).UserWorkoutId);
            Assert.Equal(workout2.WorkoutName, userDto.UserWorkouts.ElementAt(1).WorkoutName);
        }

        [Fact]
        public async Task GetUserDtoByWorkoutDate_ReturnsCorrectUserDto()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var workout1 = new UserWorkout
            {
                UserWorkoutId = "Workout 1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 20),
                CaloriesBurned = 600,
                ApplicationUserId = user.Id
            };
            var workout2 = new UserWorkout
            {
                UserWorkoutId = "Workout 2",
                WorkoutName = "Weights",
                WorkoutType = 5,
                Intensity = 2,
                Length = 75,
                WorkoutDate = new DateTime(2020, 3, 21),
                CaloriesBurned = 500,
                ApplicationUserId = user.Id
            };
            user.UserWorkouts.Add(workout1);
            user.UserWorkouts.Add(workout2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = await _repository.GetUserDtoByWorkoutDate(user.Id, "2020-3-20");

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(1, userDto.UserWorkouts.Count);
            Assert.Equal(workout1.UserWorkoutId, userDto.UserWorkouts.ElementAt(0).UserWorkoutId);
            Assert.Equal(workout1.WorkoutName, userDto.UserWorkouts.ElementAt(0).WorkoutName);
        }

        [Fact]
        public async Task GetUserWorkoutByUserWorkoutId_ReturnsCorrectUserWorkout()
        {
            // Arrange
            var workout1 = new UserWorkout
            {
                UserWorkoutId = "Workout 1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 20),
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };
            var workout2 = new UserWorkout
            {
                UserWorkoutId = "Workout 2",
                WorkoutName = "Weights",
                WorkoutType = 5,
                Intensity = 2,
                Length = 75,
                WorkoutDate = new DateTime(2020, 3, 21),
                CaloriesBurned = 500,
                ApplicationUserId = "user123"
            };
            await _context.UserWorkouts.AddAsync(workout1);
            await _context.UserWorkouts.AddAsync(workout2);
            await _context.SaveChangesAsync();

            // Act
            var userWorkout = await _repository.GetUserWorkoutByUserWorkoutId("Workout 2");

            // Assert
            Assert.NotNull(userWorkout);
            Assert.Equal(workout2.UserWorkoutId, userWorkout.UserWorkoutId);
        }

        [Fact]
        public async Task AddUserWorkout_AddsWorkout()
        {
            // Arrange
            var newWorkout = new UserWorkout
            {
                UserWorkoutId = "testableWorkout",
                WorkoutName = "Swim",
                WorkoutType = 8,
                Intensity = 1,
                Length = 30,
                WorkoutDate = new DateTime(2020, 3, 25),
                CaloriesBurned = 400,
                ApplicationUserId = "user123"
            };

            // Act
            await _repository.AddUserWorkout(newWorkout);
            await _repository.Save();
            var addedWorkout = await _repository.GetUserWorkoutByUserWorkoutId("testableWorkout");

            // Assert
            Assert.NotNull(addedWorkout);
            Assert.Equal(newWorkout.UserWorkoutId, addedWorkout.UserWorkoutId);
        }

        [Fact]
        public async Task UpdateUserWorkout_UpdatesWorkout()
        {
            // Arrange
            var newWorkout = new UserWorkout
            {
                UserWorkoutId = "testableWorkout",
                WorkoutName = "Swim",
                WorkoutType = 8,
                Intensity = 1,
                Length = 30,
                WorkoutDate = new DateTime(2020, 3, 25),
                CaloriesBurned = 400,
                ApplicationUserId = "user123"
            };

            await _repository.AddUserWorkout(newWorkout);
            await _repository.Save();
            var oldWorkout = await _repository.GetUserWorkoutByUserWorkoutId("testableWorkout");
            var oldId = oldWorkout.UserWorkoutId;
            var oldLength = oldWorkout.Length;
            var oldCaloriesBurned = oldWorkout.CaloriesBurned;

            var updatedWorkout = new UserWorkout
            {
                UserWorkoutId = "testableWorkout",
                WorkoutName = "Swim",
                WorkoutType = 8,
                Intensity = 1,
                Length = 45,
                WorkoutDate = new DateTime(2020, 3, 25),
                CaloriesBurned = 600,
                ApplicationUserId = "user123"
            };

            // Act
            await _repository.UpdateUserWorkout(updatedWorkout);
            await _repository.Save();
            var updated = await _repository.GetUserWorkoutByUserWorkoutId("testableWorkout");

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(oldId, updated.UserWorkoutId);
            Assert.NotEqual(oldLength, updated.Length);
            Assert.NotEqual(oldCaloriesBurned, updated.CaloriesBurned);
            Assert.Equal(updatedWorkout.Length, updated.Length);
            Assert.Equal(updatedWorkout.CaloriesBurned, updated.CaloriesBurned);
        }

        [Fact]
        public async Task DeleteUserWorkou_DeletessWorkout()
        {
            // Arrange
            var newWorkout = new UserWorkout
            {
                UserWorkoutId = "deletableWorkout",
                WorkoutName = "Climb",
                WorkoutType = 7,
                Intensity = 3,
                Length = 90,
                WorkoutDate = new DateTime(2020, 3, 27),
                CaloriesBurned = 900,
                ApplicationUserId = "user123"
            };

            await _repository.AddUserWorkout(newWorkout);
            await _repository.Save();
            var deletableWorkout = await _repository.GetUserWorkoutByUserWorkoutId("deletableWorkout");

            // Act
            await _repository.DeleteUserWorkout(deletableWorkout.UserWorkoutId);
            await _repository.Save();
            var afterDeleting = _repository.GetUserWorkoutByUserWorkoutId(deletableWorkout.UserWorkoutId);

            // Assert
            Assert.NotSame(deletableWorkout, afterDeleting);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}