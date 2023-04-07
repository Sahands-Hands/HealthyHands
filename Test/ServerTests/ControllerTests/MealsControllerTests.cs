///File Name MealsControllerTests.cs

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.MealsRepository;
using HealthyHands.Server.Controllers;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HealthyHands.Tests.ServerTests.ControllerTests
{
    public class MealsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly MealsRepository _repository;
        private readonly MealsController _controller;
        private readonly UserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;


        public MealsControllerTests()
        {
            // Set up a new ApplicationDbContext and MealsRepository and MealController for each test
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(_options, _operationalStoreOptions);
            _repository = new MealsRepository(_context);
            _userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(_userStore, null, null, null, null, null, null, null, null);
            _controller = new MealsController(_repository, _userManager);
        }

        [Fact]
        public async Task GetMeals_ReturnsUserDtoWithMeals()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "2", UserName = "example name" };
            var meal1 = new UserMeal
            {
                UserMealId = "1",
                MealName = "Test Meal",
                MealDate = new DateTime(2020, 3, 20),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            var meal2 = new UserMeal
            {
                UserMealId = "2",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 600,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            newUser.UserMeals.Add(meal1);
            newUser.UserMeals.Add(meal2);
            await _userManager.CreateAsync(newUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var actionResult = await _controller.GetMeals();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(2, userDto.UserMeals.Count);
            Assert.Equal(newUser.UserMeals.ElementAt<UserMeal>(0), userDto.UserMeals.ElementAt<UserMeal>(0));
            Assert.Equal(newUser.UserMeals.ElementAt<UserMeal>(1), userDto.UserMeals.ElementAt<UserMeal>(1));
            Assert.Equal(newUser.UserMeals.ElementAt<UserMeal>(0).Calories, userDto.UserMeals.ElementAt<UserMeal>(0).Calories);
        }

        [Fact]
        public async Task GetMealDate_ReturnCorrectMeal()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "123", UserName = "example name" };
            var meal3 = new UserMeal
            {
                UserMealId = "1",
                MealName = "Test Meal 3",
                MealDate = new DateTime(2020, 3, 23),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            var meal4 = new UserMeal
            {
                UserMealId = "2",
                MealName = "Test Meal 4",
                MealDate = new DateTime(2020, 3, 25),
                Calories = 600,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            newUser.UserMeals.Add(meal3);
            newUser.UserMeals.Add(meal4);
            await _userManager.CreateAsync(newUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var actionResult = await _controller.GetByMealDate("2020-3-25");
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserMeals.Count);
            Assert.Equal(newUser.UserMeals.ElementAt<UserMeal>(1), userDto.UserMeals.ElementAt<UserMeal>(0));
            Assert.Equal(newUser.UserMeals.ElementAt<UserMeal>(1).Calories, userDto.UserMeals.ElementAt<UserMeal>(0).Calories);
        }

        [Fact]
        public async Task AddMeal_ReturnsAddedMeal()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "add_user", UserName = "example name" };
            var meal5 = new UserMealDto
            {
                UserMealId = "5",
                MealName = "Test Meal 5",
                MealDate = new DateTime(2020, 3, 20),
                Calories = 800,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            await _userManager.CreateAsync(newUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddMeal(meal5);
            var actionResult = await _controller.GetMeals();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.NotNull(userDto);
            Assert.Single(userDto.UserMeals);
            Assert.Equal(meal5.Calories, userDto.UserMeals.ElementAt<UserMeal>(0).Calories);
            Assert.Equal(meal5.MealName, userDto.UserMeals.ElementAt<UserMeal>(0).MealName);
        }

        [Fact]
        public async Task UpdateMeal_UpdatesAddedMeal()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "update_user", UserName = "example name" };
            var oldMeal = new UserMealDto
            {
                UserMealId = "1",
                MealName = "Test Meal 6",
                MealDate = new DateTime(2020, 3, 20),
                Calories = 777,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            var updatedMeal = new UserMealDto
            {
                UserMealId = "2",
                MealName = "Test Meal 7",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 888,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            await _userManager.CreateAsync(newUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddMeal(oldMeal);
            var oldActionResult = await _controller.GetMeals();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            updatedMeal.UserMealId = oldUserDto.UserMeals.ElementAt<UserMeal>(0).UserMealId;
            var okResultUpdated = await _controller.UpdateMeal(updatedMeal);
            var actionResult = await _controller.GetMeals();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultUpdated);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserMeals.Count);
            Assert.Equal(updatedMeal.Calories, userDto.UserMeals.ElementAt<UserMeal>(0).Calories);
            Assert.Equal(updatedMeal.MealName, userDto.UserMeals.ElementAt<UserMeal>(0).MealName);
            Assert.NotEqual(oldMeal.Calories, userDto.UserMeals.ElementAt<UserMeal>(0).Calories);
            Assert.NotEqual(oldMeal.MealName, userDto.UserMeals.ElementAt<UserMeal>(0).MealName);
        }

        [Fact]
        public async Task DeleteMeal_DeletesAddedMeal()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "delete_user", UserName = "example name" };
            var deletedMeal = new UserMealDto
            {
                UserMealId = "2",
                MealName = "Test Meal 8",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 999,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = newUser.Id
            };
            await _userManager.CreateAsync(newUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var okResult = await _controller.AddMeal(deletedMeal);
            var oldActionResult = await _controller.GetMeals();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            string mealId = oldUserDto.UserMeals.ElementAt<UserMeal>(0).UserMealId;
            var okResultDeleted = await _controller.DeleteMeal(mealId);
            var actionResult = await _controller.GetMeals();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultDeleted);
            Assert.NotNull(userDto);
            Assert.NotNull(mealId);
            Assert.Equal(0, userDto.UserMeals.Count);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
