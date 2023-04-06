///File Name UserWeightsTests.cs

using HealthyHands.Server.Data;
using HealthyHands.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Duende.IdentityServer.EntityFramework.Options;

namespace HealthyHands.Tests.ServerTests.DataTests
{
    public class UserWeightsTests
    {

        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public UserWeightsTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
        }

        [Fact]
        public async Task TestUserWeightsCRUD()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options, _operationalStoreOptions);
            var userWeight = new UserWeight
            {
                UserWeightId = "1",
                Weight = 180,
                WeightDate = DateTime.UtcNow,
                ApplicationUserId = "a"
            };

            // Act: Add UserWeight
            context.UserWeights.Add(userWeight);
            await context.SaveChangesAsync();

            // Assert: UserWeight is added
            var addedUserWeight = await context.UserWeights.FindAsync(userWeight.UserWeightId);
            Assert.NotNull(addedUserWeight);
            Assert.Equal(userWeight.Weight, addedUserWeight.Weight);

            // Act: Update UserWeight
            addedUserWeight.Weight = 170;
            context.UserWeights.Update(addedUserWeight);
            await context.SaveChangesAsync();

            // Assert: UserWeight is updated
            var updatedUserWeight = await context.UserWeights.FindAsync(userWeight.UserWeightId);
            Assert.NotNull(updatedUserWeight);
            Assert.Equal(170, updatedUserWeight.Weight);

            // Act: Delete UserWeight
            context.UserWeights.Remove(updatedUserWeight);
            await context.SaveChangesAsync();

            // Assert: UserWeight is deleted
            var deletedUserWeight = await context.UserWeights.FindAsync(userWeight.UserWeightId);
            Assert.Null(deletedUserWeight);
        }

    }
}
