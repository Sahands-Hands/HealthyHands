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
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthyHands.Tests
{
    public class MealsControllerTests
    {
        private readonly MealsController _mealsController;
        private readonly Mock<IMealsRepository> _mockMealsRepository;

        public MealsControllerTests()
        {
            _mockMealsRepository = new Mock<IMealsRepository>();
            _mealsController = new MealsController(_mockMealsRepository.Object);
        }

        [Fact]
        public async void GetMeals_ReturnsOkObjectResult_WhenMealsExist()
        {
            // Arrange
            string userId = "some-user-id";
            var userDto = new UserDto {Id = userId, UserMeals = new List<UserMeal> { new UserMeal { UserMealId = "1", MealName = "Breakfast", MealDate = new System.DateTime(2023, 03, 31), ApplicationUserId = userId } } };
            _mockMealsRepository.Setup(repo => repo.GetUserDtoWithAllMeals(userId)).Returns(userDto);

            // Act
            var result = await _mealsController.GetMeals();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var meals = okResult.Value as IEnumerable<UserMeal>;
            Assert.NotNull(meals);
            Assert.Equal(1, meals.Count());
        }

        [Fact]
        public async void GetMeals_ReturnsNotFoundResult_WhenMealsDoNotExist()
        {
            // Arrange
            string userId = "some-user-id";
            _mockMealsRepository.Setup(repo => repo.GetUserDtoWithAllMeals(userId)).Returns(null as UserDto);

            // Act
            var result = await _mealsController.GetMeals();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}