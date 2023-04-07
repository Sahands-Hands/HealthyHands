///File Name MealRepositoryTests.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.MealsRepository;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;

namespace HealthyHands.Tests.ServerTests.DataTests
{
    public class MealRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly MealsRepository _repository;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public MealRepositoryTests()
        {
            // Set up a new ApplicationDbContext and WorkoutsRepository for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(options, operationalStoreOptions);
            _repository = new MealsRepository(_context);
        }

        [Fact]
        public async Task GetUserDtoWithAllMeals_ReturnsCorrectUserDto()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var meal1 = new UserMeal
            {
                UserMealId = "1",
                MealName = "Test Meal",
                MealDate = new DateTime(2020,3,20),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };
            var meal2 = new UserMeal
            {
                UserMealId = "2",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020,3,21),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };
            user.UserMeals.Add(meal1);
            user.UserMeals.Add(meal2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = _repository.GetUserDtoWithAllMeals(user.Id).Result;

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(2, userDto.UserMeals.Count);
            Assert.Equal(meal1.UserMealId, userDto.UserMeals.ElementAt(0).UserMealId);
            Assert.Equal(meal1.MealName, userDto.UserMeals.ElementAt(0).MealName);
            Assert.Equal(meal2.UserMealId, userDto.UserMeals.ElementAt(1).UserMealId);
            Assert.Equal(meal2.MealName, userDto.UserMeals.ElementAt(1).MealName);
        }

        [Fact]
        public async Task GetUserDtoByMealDate_ReturnsCorrectUserMeal()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
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
                ApplicationUserId = "testuser"
            };
            var meal2 = new UserMeal
            {
                UserMealId = "2",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };
            user.UserMeals.Add(meal1);
            user.UserMeals.Add(meal2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = _repository.GetUserDtoByMealDate(user.Id, "2020-3-20").Result;

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(1, userDto.UserMeals.Count);
            Assert.Equal(meal1.UserMealId, userDto.UserMeals.ElementAt(0).UserMealId);
            Assert.Equal(meal1.MealName, userDto.UserMeals.ElementAt(0).MealName);
        }

        [Fact]
        public async Task GetUserMealByUserMealId_ReturnsCorrectUserMeal()
        {
            // Arrange
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
                ApplicationUserId = "testuser"
            };
            var meal2 = new UserMeal
            {
                UserMealId = "2",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };
            await _context.UserMeals.AddAsync(meal1);
            await _context.UserMeals.AddAsync(meal2);
            await _context.SaveChangesAsync();

            // Act
            var userMeal = _repository.GetUserMealByUserMealId("2");

            // Assert
            Assert.NotNull(userMeal);
            Assert.Equal(meal2.UserMealId, userMeal.UserMealId);
        }

        [Fact]
        public async Task AddUserMeal_AddsMeal()
        {
            // Arrange
            var newMeal = new UserMeal
            {
                UserMealId = "addable",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };

            // Act
             await _repository.AddUserMeal(newMeal);
             await _repository.Save();
            var addedMeal = _repository.GetUserMealByUserMealId("addable");

            // Assert
            Assert.NotNull(addedMeal);
            Assert.Equal(newMeal.UserMealId, addedMeal.UserMealId);
        }

        [Fact]
        public async Task UpdateUserMeal_UpdatesMeal()
        {
            // Arrange
            var newMeal = new UserMeal
            {
                UserMealId = "updatable",
                MealName = "Test Meal 2",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };

            await _repository.AddUserMeal(newMeal);
            await _repository.Save();
            var oldMeal = _repository.GetUserMealByUserMealId("updatable");
            var oldId = oldMeal.UserMealId;
            var oldName = oldMeal.MealName;
            var oldCalories = oldMeal.Calories;

            var updatedMeal = new UserMeal
            {
                UserMealId = "updatable",
                MealName = "Updated Meal",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 600,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };

            // Act
            await _repository.UpdateUserMeal(updatedMeal);
            await _repository.Save();
            var updated = _repository.GetUserMealByUserMealId("updatable");

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(oldId, updated.UserMealId);
            Assert.NotEqual(oldName, updated.MealName);
            Assert.NotEqual(oldCalories, updated.Calories);
            Assert.Equal(updatedMeal.MealName, updated.MealName);
            Assert.Equal(updatedMeal.Calories, updated.Calories);
        }

        [Fact]
        public async Task DeleteUserMeal_DeletesMeal()
        {
            // Arrange
            var newMeal = new UserMeal
            {
                UserMealId = "deletable",
                MealName = "Test Meal",
                MealDate = new DateTime(2020, 3, 21),
                Calories = 600,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20,
                ApplicationUserId = "testuser"
            };

            await _repository.AddUserMeal(newMeal);
            await _repository.Save();
            var deletableMeal = _repository.GetUserMealByUserMealId("deletable");

            // Act
            await _repository.DeleteUserMeal(deletableMeal.UserMealId);
            await _repository.Save();
            var afterDeleting = _repository.GetUserMealByUserMealId(deletableMeal.UserMealId);

            // Assert
            Assert.NotSame(deletableMeal, afterDeleting);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}