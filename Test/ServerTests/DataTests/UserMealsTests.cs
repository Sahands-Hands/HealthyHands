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
    public class UserMealTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public UserMealTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
        }

        [Fact]
        public async Task TestUserMealsCRUD()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options, _operationalStoreOptions);
            var userMeal = new UserMeal
            {
                UserMealId = "1",
                MealName = "Test Meal",
                MealDate = DateTime.UtcNow,
                Calories = 500,
                Protein = 30,
                Carbs = 60,
                Fat = 10,
                Sugar = 20
            };

            // Act: Add UserMeal
            context.UserMeals.Add(userMeal);
            await context.SaveChangesAsync();

            // Assert: UserMeal is added
            var addedUserMeal = await context.UserMeals.FindAsync(userMeal.UserMealId);
            Assert.NotNull(addedUserMeal);
            Assert.Equal(userMeal.MealName, addedUserMeal.MealName);

            // Act: Update UserMeal
            addedUserMeal.MealName = "Updated Test Meal";
            context.UserMeals.Update(addedUserMeal);
            await context.SaveChangesAsync();

            // Assert: UserMeal is updated
            var updatedUserMeal = await context.UserMeals.FindAsync(userMeal.UserMealId);
            Assert.NotNull(updatedUserMeal);
            Assert.Equal("Updated Test Meal", updatedUserMeal.MealName);

            // Act: Delete UserMeal
            context.UserMeals.Remove(updatedUserMeal);
            await context.SaveChangesAsync();

            // Assert: UserMeal is deleted
            var deletedUserMeal = await context.UserMeals.FindAsync(userMeal.UserMealId);
            Assert.Null(deletedUserMeal);
        }
    }
}