// FileName: MealsRepository.cs

using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.MealsRepository
{
    /// <summary>
    /// The meals repository.
    /// </summary>
    public class MealsRepository : IMealsRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealsRepository"/> class.
        /// </summary>
        /// <param name="context">The db context.</param>
        public MealsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the user dto with all meals.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public async Task<UserDto> GetUserDtoWithAllMeals(string userId)
        {
            var user = await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserMeals = u.UserMeals
            }).FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        /// <summary>
        /// Gets the user dto filtered by meal date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="mealDate">The meal date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public async Task<UserDto> GetUserDtoByMealDate(string userId, string mealDate)
        {
            DateTime mealDateTime;
            DateTime.TryParse(mealDate, out mealDateTime);

            var user = await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserMeals = u.UserMeals
            }).FirstOrDefaultAsync(u => u.Id == userId);

            user.UserMeals = user.UserMeals.Where(u => u.MealDate.Date == mealDateTime.Date).Select(m => m).ToList();

            return user;
        }

        /// <summary>
        /// Gets the user meal by user meal id.
        /// </summary>
        /// <param name="userMealId">The user meal id.</param>
        /// <returns>A <see cref="UserMeal"/>.</returns>
        public UserMeal GetUserMealByUserMealId(string userMealId)
        {
            var meal = _context.UserMeals.Select(m => m).FirstOrDefault(m => m.UserMealId == userMealId);
            return meal;
        }

        /// <summary>
        /// Adds the user meal.
        /// </summary>
        /// <param name="userMeal">The user meal.</param>
        public async Task AddUserMeal(UserMeal userMeal)
        {
            await _context.UserMeals.AddAsync(userMeal);
        }

        /// <summary>
        /// Updates the user meal.
        /// </summary>
        /// <param name="userMeal">The user meal.</param>
        public async Task UpdateUserMeal(UserMeal userMeal)
        {
            var mealToUpdate = await _context.UserMeals.Select(m => m).FirstOrDefaultAsync(m => m.UserMealId == userMeal.UserMealId && m.ApplicationUserId == userMeal.ApplicationUserId);

            mealToUpdate.MealName = userMeal.MealName;
            mealToUpdate.MealDate = userMeal.MealDate;
            mealToUpdate.Calories = userMeal.Calories;
            mealToUpdate.Protein = userMeal.Protein;
            mealToUpdate.Carbs = userMeal.Carbs;
            mealToUpdate.Fat = userMeal.Fat;
            mealToUpdate.Sugar = userMeal.Sugar;

            _context.Update(mealToUpdate);
        }

        /// <summary>
        /// Deletes the user meal.
        /// </summary>
        /// <param name="userMealId">The user meal id.</param>
        public async Task DeleteUserMeal(string userMealId)
        {
            var userMealToRemove = await _context.UserMeals.Select(m => m)
                .FirstOrDefaultAsync(m => m.UserMealId == userMealId);
            if (userMealToRemove != null)
            {
               _context.UserMeals.Remove(userMealToRemove);
            }
        }

        /// <summary>
        /// Saves changes made to the <see cref="ApplicationDbContext"/>.
        /// </summary>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        /// <summary>
        /// Logic to dispose the <see cref="ApplicationDbContext"/>.
        /// </summary>
        /// <param name="disposing">If true, disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        /// <summary>
        /// Disposes the <see cref="ApplicationDbContext"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
