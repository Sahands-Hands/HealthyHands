///File Name WeightControllerTests.cs

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WeightsRepository;
using HealthyHands.Server.Controllers;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HealthyHands.Tests.ServerTests.ControllerTests
{
    public class WeightControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly WeightsRepository _repository;
        private readonly WeightsController _controller;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private readonly UserManager<ApplicationUser> _userManager;   

        public WeightControllerTests()
        {
            // Set up a new ApplicationDbContext and WeightsRepository and WeightController for each test
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _operationalStoreOptions = Options.Create(new OperationalStoreOptions());
            _context = new ApplicationDbContext(_options, _operationalStoreOptions);
            _repository = new WeightsRepository(_context);
            _controller = new WeightsController(_repository, _userManager);
        }

        [Fact]
        public async Task GetWeights_ReturnsUserDtoWithWeights()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "2", UserName = "example name" };
            var weight1 = new UserWeight
            {
                UserWeightId = "1",
                Weight = 180,
                WeightDate = new DateTime(2020, 3, 20),
                ApplicationUserId = newUser.Id
            };
            var weight2 = new UserWeight
            {
                UserWeightId = "2",
                Weight = 175,
                WeightDate = new DateTime(2020, 3, 21),
                ApplicationUserId = newUser.Id
            };
            newUser.UserWeights.Add(weight1);
            newUser.UserWeights.Add(weight2);
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
            var actionResult = await _controller.GetWeights();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(2, userDto.UserWeights.Count);
            Assert.Equal(newUser.UserWeights.ElementAt<UserWeight>(0), userDto.UserWeights.ElementAt<UserWeight>(0));
            Assert.Equal(newUser.UserWeights.ElementAt<UserWeight>(1), userDto.UserWeights.ElementAt<UserWeight>(1));
            Assert.Equal(newUser.UserWeights.ElementAt<UserWeight>(0).Weight, userDto.UserWeights.ElementAt<UserWeight>(0).Weight);
        }

        [Fact]
        public async Task GetWeightDate_ReturnCorrectWeight()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "123", UserName = "example name" };
            var weight3 = new UserWeight
            {
                UserWeightId = "3",
                Weight = 180,
                WeightDate = new DateTime(2020, 3, 22),
                ApplicationUserId = newUser.Id
            };
            var weight4 = new UserWeight
            {
                UserWeightId = "4",
                Weight = 175,
                WeightDate = new DateTime(2020, 3, 23),
                ApplicationUserId = newUser.Id
            };
            newUser.UserWeights.Add(weight3);
            newUser.UserWeights.Add(weight4);
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
            var actionResult = await _controller.GetByWeightDate("2020-3-23");
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWeights.Count);
            Assert.Equal(newUser.UserWeights.ElementAt<UserWeight>(1), userDto.UserWeights.ElementAt<UserWeight>(0));
            Assert.Equal(newUser.UserWeights.ElementAt<UserWeight>(1).Weight, userDto.UserWeights.ElementAt<UserWeight>(0).Weight);
        }

        [Fact]
        public async Task AddWeight_ReturnsAddedWeight()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "add_user", UserName = "example name" };
            var weight5 = new UserWeightDto
            {
                UserWeightId = "5",
                Weight = 170,
                WeightDate = new DateTime(2020, 3, 25),
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
            var okResult = await _controller.AddWeight(weight5);
            var actionResult = await _controller.GetWeights();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWeights.Count);
            Assert.Equal(weight5.WeightDate, userDto.UserWeights.ElementAt<UserWeight>(0).WeightDate);
            Assert.Equal(weight5.Weight, userDto.UserWeights.ElementAt<UserWeight>(0).Weight);
        }

        [Fact]
        public async Task UpdateWeight_UpdatesAddedWeight()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "update_user", UserName = "example name" };
            var oldWeight = new UserWeightDto
            {
                UserWeightId = "6",
                Weight = 188,
                WeightDate = new DateTime(2020, 3, 25),
                ApplicationUserId = newUser.Id
            };
            var updatedWeight = new UserWeightDto
            {
                UserWeightId = "7",
                Weight = 177,
                WeightDate = new DateTime(2020, 3, 27),
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
            var okResult = await _controller.AddWeight(oldWeight);
            var oldActionResult = await _controller.GetWeights();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            updatedWeight.UserWeightId = oldUserDto.UserWeights.ElementAt<UserWeight>(0).UserWeightId;
            var okResultUpdated = await _controller.UpdateWeight(updatedWeight);
            var actionResult = await _controller.GetWeights();
            var result = actionResult.Result as OkObjectResult;
            UserDto? userDto = (UserDto)result.Value;

            // Assert
            Assert.NotNull(result.Value);
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultUpdated);
            Assert.NotNull(userDto);
            Assert.Equal(1, userDto.UserWeights.Count);
            Assert.Equal(updatedWeight.WeightDate, userDto.UserWeights.ElementAt<UserWeight>(0).WeightDate);
            Assert.Equal(updatedWeight.Weight, userDto.UserWeights.ElementAt<UserWeight>(0).Weight);
            Assert.NotEqual(oldWeight.WeightDate, userDto.UserWeights.ElementAt<UserWeight>(0).WeightDate);
            Assert.NotEqual(oldWeight.Weight, userDto.UserWeights.ElementAt<UserWeight>(0).Weight);
        }

        [Fact]
        public async Task DeleteWeight_DeletesAddedWeight()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "delete_user", UserName = "example name" };
            var deleteWeight = new UserWeightDto
            {
                UserWeightId = "7",
                Weight = 111,
                WeightDate = new DateTime(2020, 3, 29),
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
            var okResult = await _controller.AddWeight(deleteWeight);
            var oldActionResult = await _controller.GetWeights();
            var oldResult = oldActionResult.Result as OkObjectResult;
            UserDto? oldUserDto = (UserDto)oldResult.Value;
            string weightId = oldUserDto.UserWeights.ElementAt<UserWeight>(0).UserWeightId;
            var okResultDeleted = await _controller.DeleteWeight(weightId);
            var actionResult = await _controller.GetWeights();

            // Assert
            Assert.IsType<OkResult>(okResult);
            Assert.IsType<OkResult>(okResultDeleted);
            Assert.NotNull(oldUserDto);
            Assert.NotNull(weightId);
        }

        public void Dispose()
        {
            // Clean up the ApplicationDbContext after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}