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

namespace HealthyHands.Tests.ServerTests.ModelTests
{
    public class UserMealTests
    {
        [Fact]
        public void UserMeal_GettersAndSetters_ReturnExpectedValues()
        {
            // Arrange
            var userMeal = new UserMeal();
            userMeal.UserMealId = "1";
            userMeal.MealName = "Breakfast";
            userMeal.MealDate = DateTime.Now.Date;
            userMeal.Calories = 500;
            userMeal.Protein = 30;
            userMeal.Carbs = 50;
            userMeal.Fat = 20;
            userMeal.Sugar = 10;
            userMeal.ApplicationUserId = "user123";

            // Act
            var userMealId = userMeal.UserMealId;
            var mealName = userMeal.MealName;
            var mealDate = userMeal.MealDate;
            var calories = userMeal.Calories;
            var protein = userMeal.Protein;
            var carbs = userMeal.Carbs;
            var fat = userMeal.Fat;
            var sugar = userMeal.Sugar;
            var applicationUserId = userMeal.ApplicationUserId;

            // Assert
            Assert.Equal("1", userMealId);
            Assert.Equal("Breakfast", mealName);
            Assert.Equal(DateTime.Now.Date, mealDate);
            Assert.Equal(500, calories);
            Assert.Equal(30, protein);
            Assert.Equal(50, carbs);
            Assert.Equal(20, fat);
            Assert.Equal(10, sugar);
            Assert.Equal("user123", applicationUserId);
        }
    }
}
