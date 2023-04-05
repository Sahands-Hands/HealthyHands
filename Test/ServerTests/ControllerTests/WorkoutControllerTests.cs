///File Name WorkoutControllerTests.cs

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WorkoutsRepository;
using HealthyHands.Server.Controllers;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HealthyHands.Tests.ServerTests.ControllerTests
{
    public class WorkoutControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkoutsRepository _repository;
        private readonly WorkoutsController _controller;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public WorkoutControllerTests()
        {
            // Set up a new ApplicationDbContext and WorkoutsRepository and WorkoutController for each test
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(_options, _operationalStoreOptions);
            _repository = new WorkoutsRepository(_context);
            _controller = new WorkoutsController(_repository);
        }

        [Fact]
        public async Task GetWorkouts_ReturnsUserDtoWithWorkouts()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "2", UserName = "example name" };
            var workout1 = new UserWorkout
            {
                UserWorkoutId = "Workout 1",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 20),
                CaloriesBurned = 600,
                ApplicationUserId = newUser.Id
            };
            var workout2 = new UserWorkout
            {
                UserWorkoutId = "Workout 2",
                WorkoutName = "Weights",
                WorkoutType = 5,
                Intensity = 2,
                Length = 75,
                WorkoutDate = new DateTime(2020, 3, 20),
                CaloriesBurned = 500,
                ApplicationUserId = newUser.Id
            };
            newUser.UserWorkouts.Add(workout1);
            newUser.UserWorkouts.Add(workout2);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var actionResult = await _controller.GetWorkouts();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(2, userDto.UserWorkouts.Count);
            Assert.Equal(newUser.UserWorkouts.ElementAt<UserWorkout>(0), userDto.UserWorkouts.ElementAt<UserWorkout>(0));
            Assert.Equal(newUser.UserWorkouts.ElementAt<UserWorkout>(1), userDto.UserWorkouts.ElementAt<UserWorkout>(1));
            Assert.Equal(newUser.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned, userDto.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned);
        }

        [Fact]
        public async Task GetWorkoutDate_ReturnCorrectWorkout()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "123", UserName = "example name" };
            var workout3 = new UserWorkout
            {
                UserWorkoutId = "Workout 3",
                WorkoutName = "Jog",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 22),
                CaloriesBurned = 600,
                ApplicationUserId = newUser.Id
            };
            var workout4 = new UserWorkout
            {
                UserWorkoutId = "Workout 4",
                WorkoutName = "Weights",
                WorkoutType = 5,
                Intensity = 2,
                Length = 75,
                WorkoutDate = new DateTime(2020, 3, 23),
                CaloriesBurned = 500,
                ApplicationUserId = newUser.Id
            };
            newUser.UserWorkouts.Add(workout3);
            newUser.UserWorkouts.Add(workout4);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var actionResult = await _controller.GetByWorkoutDate("2020-3-23");
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWorkouts.Count);
            Assert.Equal(newUser.UserWorkouts.ElementAt<UserWorkout>(1), userDto.UserWorkouts.ElementAt<UserWorkout>(0));
            Assert.Equal(newUser.UserWorkouts.ElementAt<UserWorkout>(1).CaloriesBurned, userDto.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned);
        }

        [Fact]
        public async Task AddWorkout_ReturnsAddedWorkout()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "add_user", UserName = "example name" };
            var workout5 = new UserWorkoutDto
            {
                UserWorkoutId = "Workout 5",
                WorkoutName = "Cycling",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 22),
                CaloriesBurned = 777,
                ApplicationUserId = newUser.Id
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddWorkout(workout5);
            var actionResult = await _controller.GetWorkouts();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWorkouts.Count);
            Assert.Equal(workout5.CaloriesBurned, userDto.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned);
            Assert.Equal(workout5.WorkoutName, userDto.UserWorkouts.ElementAt<UserWorkout>(0).WorkoutName);
        }

        [Fact]
        public async Task UpdateWorkout_UpdatesAddedWorkout()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "update_user", UserName = "example name" };
            var oldWorkout = new UserWorkoutDto
            {
                UserWorkoutId = "Workout 6",
                WorkoutName = "Skating",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 22),
                CaloriesBurned = 888,
                ApplicationUserId = newUser.Id
            };
            var updatedWorkout = new UserWorkoutDto
            {
                UserWorkoutId = "Workout 6",
                WorkoutName = "Skiing",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 22),
                CaloriesBurned = 999,
                ApplicationUserId = newUser.Id
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddWorkout(oldWorkout);
            var oldActionResult = await _controller.GetWorkouts();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            updatedWorkout.UserWorkoutId = oldUserDto.UserWorkouts.ElementAt<UserWorkout>(0).UserWorkoutId;
            var okResultUpdated = await _controller.UpdateWorkout(updatedWorkout);
            var actionResult = await _controller.GetWorkouts();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultUpdated);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWorkouts.Count);
            Assert.Equal(updatedWorkout.CaloriesBurned, userDto.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned);
            Assert.Equal(updatedWorkout.WorkoutName, userDto.UserWorkouts.ElementAt<UserWorkout>(0).WorkoutName);
            Assert.NotEqual(oldWorkout.CaloriesBurned, userDto.UserWorkouts.ElementAt<UserWorkout>(0).CaloriesBurned);
            Assert.NotEqual(oldWorkout.WorkoutName, userDto.UserWorkouts.ElementAt<UserWorkout>(0).WorkoutName);
        }

        [Fact]
        public async Task DeleteWorkout_DeletesAddedWorkout()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "update_user", UserName = "example name" };
            var deletableWorkout = new UserWorkoutDto
            {
                UserWorkoutId = "Workout 7",
                WorkoutName = "Codingg",
                WorkoutType = 2,
                Intensity = 3,
                Length = 60,
                WorkoutDate = new DateTime(2020, 3, 22),
                CaloriesBurned = 111,
                ApplicationUserId = newUser.Id
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddWorkout(deletableWorkout);
            var oldActionResult = await _controller.GetWorkouts();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            string workoutId = oldUserDto.UserWorkouts.ElementAt<UserWorkout>(0).UserWorkoutId;
            var okResultDeleted = await _controller.DeleteWorkout(workoutId);
            var actionResult = await _controller.GetWorkouts();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultDeleted);
            Assert.NotNull(userDto);
            Assert.NotNull(workoutId);
            Assert.Equal(0, userDto.UserWorkouts.Count);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
