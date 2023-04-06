// Filename: MealsController.cs

using System.Security.Claims;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.MealsRepository;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Controllers
{
    /// <summary>
    /// The meals controller.
    /// </summary>
    [Authorize]
    [Route("meals")]
    public class MealsController : Controller
    {
        private readonly IMealsRepository _mealsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealsController"/> class.
        /// </summary>
        /// <param name="mealsRepository">The meals repository.</param>
        public MealsController(IMealsRepository mealsRepository, UserManager<ApplicationUser> userManager)
        {
            _mealsRepository = mealsRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all meals for logged in user.
        /// </summary>
        /// <returns>A <see cref="UserDto"/> with meals</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<UserDto>> GetMeals()
        {
            UserDto? user;
            var userId = _userManager.GetUserAsync(User).Result.Id;
            try
            {
                user = await _mealsRepository.GetUserDtoWithAllMeals(userId);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets all meals for logged in user filtered by meal date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="UserDto"/> with meals</returns>
        [HttpGet]
        [Route("byDate")]
        public async Task<ActionResult<UserDto>> GetByMealDate(string date)
        {
            UserDto? user;
            var userId = _userManager.GetUserAsync(User).Result.Id;

            try
            {
                user = await _mealsRepository.GetUserDtoByMealDate(userId, date);
            }
            catch
            {
                return BadRequest();
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Adds the meal.
        /// </summary>
        /// <param name="userMealDto">The user meal dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("add")]
        public async Task<ActionResult> AddMeal([FromBody] UserMealDto userMealDto)
        {
            UserMeal userMeal = new UserMeal
            {
                UserMealId = Guid.NewGuid().ToString(),
                MealName = userMealDto.MealName,
                MealDate = userMealDto.MealDate,
                Calories = userMealDto.Calories,
                Protein = userMealDto.Protein,
                Carbs = userMealDto.Carbs,
                Fat = userMealDto.Fat,
                Sugar = userMealDto.Sugar,
                ApplicationUserId = _userManager.GetUserAsync(User).Result.Id
            };

            try
            {
               await _mealsRepository.AddUserMeal(userMeal);
               await _mealsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Updates the meal.
        /// </summary>
        /// <param name="mealDto">The meal dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateMeal([FromBody] UserMealDto mealDto)
        { 
            var userId = _userManager.GetUserAsync(User).Result.Id;
            if (mealDto.ApplicationUserId != userId)
            {
                return NotFound();
            }

            UserMeal meal = new UserMeal
            {
                UserMealId = mealDto.UserMealId,
                MealName = mealDto.MealName,
                MealDate = mealDto.MealDate,
                Calories = mealDto.Calories,
                Protein = mealDto.Protein,
                Carbs = mealDto.Carbs,
                Fat = mealDto.Fat,
                Sugar = mealDto.Sugar,
                ApplicationUserId = userId    // Use the current user's Id!!!
            };

            try
            {
                await _mealsRepository.UpdateUserMeal(meal);
                await _mealsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes the meal.
        /// </summary>
        /// <param name="userMealId">The user meal id.</param>
        /// <returns>An Http Status Code</returns>
        [HttpDelete]
        [Route("delete/{userMealId}")]
        public async Task<ActionResult> DeleteMeal(string userMealId)
        {
            UserMeal mealToDelete = _mealsRepository.GetUserMealByUserMealId(userMealId);
            if (mealToDelete == null || mealToDelete.ApplicationUserId != _userManager.GetUserAsync(User).Result.Id)
            {
                return NotFound();
            }

            try
            {
                await _mealsRepository.DeleteUserMeal(userMealId);
                await _mealsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Disposes the <see cref="MealsRepository"/>.
        /// </summary>
        /// <param name="disposing">If true, disposing.</param>
        protected override void Dispose(bool disposing)
        {
            _mealsRepository.Dispose();
            _userManager.Dispose();
            base.Dispose(disposing);
        }
    }


}
