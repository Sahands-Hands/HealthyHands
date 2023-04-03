///File Name WeightRepositoryTests.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WeightsRepository;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;

namespace HealthyHands.Tests.ServerTests.DataTests
{
    public class WeightRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly WeightsRepository _repository;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public WeightRepositoryTests()
        {
            // Set up a new ApplicationDbContext and WeightsRepository for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(options, operationalStoreOptions);
            _repository = new WeightsRepository(_context);
        }

        [Fact]
        public async Task GetUserDtoWithAllWeights_ReturnsCorrectUserDto()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var weight1 = new UserWeight
            {
                UserWeightId = "1",
                Weight = 180,
                WeightDate = new DateTime(2020,3,20),
                ApplicationUserId = "testuser"
            };
            var weight2 = new UserWeight
            {
                UserWeightId = "2",
                Weight = 175,
                WeightDate = new DateTime(2020, 3, 21),
                ApplicationUserId = "testuser"
            };
            user.UserWeights.Add(weight1);
            user.UserWeights.Add(weight2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = await _repository.GetUserDtoWithAllWeights(user.Id);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(2, userDto.UserWeights.Count);
            Assert.Equal(weight1.UserWeightId, userDto.UserWeights.ElementAt(0).UserWeightId);
            Assert.Equal(weight1.Weight, userDto.UserWeights.ElementAt(0).Weight);
            Assert.Equal(weight2.UserWeightId, userDto.UserWeights.ElementAt(1).UserWeightId);
            Assert.Equal(weight2.Weight, userDto.UserWeights.ElementAt(1).Weight);
        }

        [Fact]
        public async Task GetUserDtoByWeightDate_ReturnsCorrectUserDto()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var weight1 = new UserWeight
            {
                UserWeightId = "1",
                Weight = 180,
                WeightDate = new DateTime(2020, 3, 20),
                ApplicationUserId = "testuser"
            };
            var weight2 = new UserWeight
            {
                UserWeightId = "2",
                Weight = 175,
                WeightDate = new DateTime(2020, 3, 21),
                ApplicationUserId = "testuser"
            };
            user.UserWeights.Add(weight1);
            user.UserWeights.Add(weight2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var userDto = await _repository.GetUserDtoByWeightDate(user.Id, "2020-3-20");

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal(user.Id, userDto.Id);
            Assert.Equal(1, userDto.UserWeights.Count);
            Assert.Equal(weight1.UserWeightId, userDto.UserWeights.ElementAt(0).UserWeightId);
            Assert.Equal(weight1.Weight, userDto.UserWeights.ElementAt(0).Weight);
        }

        [Fact]
        public async Task GetUserWeightByUserWeightId_ReturnsCorrectUserWeight()
        {
            // Arrange
            var weight1 = new UserWeight
            {
                UserWeightId = "1",
                Weight = 180,
                WeightDate = new DateTime(2020, 3, 20),
                ApplicationUserId = "testuser"
            };
            var weight2 = new UserWeight
            {
                UserWeightId = "2",
                Weight = 175,
                WeightDate = new DateTime(2020, 3, 21),
                ApplicationUserId = "testuser"
            };
            await _context.UserWeights.AddAsync(weight1);
            await _context.UserWeights.AddAsync(weight2);
            await _context.SaveChangesAsync();

            // Act
            var userWeight = _repository.GetUserWeightByUserWeightId("2");

            // Assert
            Assert.NotNull(userWeight);
            Assert.Equal(weight2.UserWeightId, userWeight.UserWeightId);
        }

        [Fact]
        public async Task AddUserWeight_AddsWeight()
        {
            // Arrange
            var newWeight = new UserWeight
            {
                UserWeightId = "3",
                Weight = 170,
                WeightDate = new DateTime(2020, 3, 25),
                ApplicationUserId = "testuser"
            };

            // Act
            await _repository.AddUserWeight(newWeight);
            await _repository.Save();
            var addedWeight = _repository.GetUserWeightByUserWeightId("3");

            // Assert
            Assert.NotNull(addedWeight);
            Assert.Equal(newWeight.UserWeightId, addedWeight.UserWeightId);
        }

        [Fact]
        public async Task UpdateUserWeight_UpdatesWeight()
        {
            // Arrange
            var newWeight = new UserWeight
            {
                UserWeightId = "updatable",
                Weight = 170,
                WeightDate = new DateTime(2020, 3, 25),
                ApplicationUserId = "testuser"
            };

            await _repository.AddUserWeight(newWeight);
            await _repository.Save();
            var oldWeight = _repository.GetUserWeightByUserWeightId("updatable");
            var oldId = oldWeight.UserWeightId;
            var oldWeightNum = oldWeight.Weight;

            var updatedWeight = new UserWeight
            {
                UserWeightId = "updatable",
                Weight = 180,
                WeightDate = new DateTime(2020, 3, 25),
                ApplicationUserId = "testuser"
            };

            // Act
            await _repository.UpdateUserWeight(updatedWeight);
            await _repository.Save();
            var updated = _repository.GetUserWeightByUserWeightId("updatable");

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(oldId, updated.UserWeightId);
            Assert.NotEqual(oldWeightNum, updated.Weight);
            Assert.Equal(updatedWeight.Weight, updated.Weight);
        }

        [Fact]
        public async Task DeleteUserWeight_DeletesWeight()
        {
            // Arrange
            var newWeight = new UserWeight
            {
                UserWeightId = "deletable",
                Weight = 170,
                WeightDate = new DateTime(2020, 3, 25),
                ApplicationUserId = "testuser"
            };

            await _repository.AddUserWeight(newWeight);
            await _repository.Save();
            var deletableWeight = _repository.GetUserWeightByUserWeightId("deletable");

            // Act
            await _repository.DeleteUserWeight(deletableWeight.UserWeightId);
            await _repository.Save();
            var afterDeleting = _repository.GetUserWeightByUserWeightId(deletableWeight.UserWeightId);

            // Assert
            Assert.NotSame(deletableWeight, afterDeleting);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}