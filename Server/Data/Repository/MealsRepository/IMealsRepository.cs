// Filename: IMealsRepository.cs

using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.MealsRepository
{
    /// <summary>
    /// The meals repository interface.
    /// </summary>
    public interface IMealsRepository : IDisposable
    {

        /// <summary>
        /// Gets the user dto with all meals.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoWithAllMeals(string userId);

        /// <summary>
        /// Gets the user dto by meal date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="mealDate">The meal date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoByMealDate(string userId, string mealDate);

        /// <summary>
        /// Gets the user meal by user meal id.
        /// This method should NOT be used in the controller to return a <see cref="UserMeal"/> object to the client.
        /// </summary>
        /// <param name="userMealId">The user meal id.</param>
        /// <returns>A <see cref="UserMeal"/>.</returns>
        UserMeal GetUserMealByUserMealId(string userMealId);

        /// <summary>
        /// Adds the user meal.
        /// </summary>
        /// <param name="userMeal">The user meal.</param>
        void AddUserMeal(UserMeal userMeal);

        /// <summary>
        /// Updates the user meal.
        /// </summary>
        /// <param name="userMeal">The user meal.</param>
        void UpdateUserMeal(UserMeal userMeal);

        /// <summary>
        /// Deletes the user meal.
        /// </summary>
        /// <param name="userMealId">The user meal id.</param>
        void DeleteUserMeal(string userMealId);

        /// <summary>
        /// Saves changes to the <see cref="ApplicationDbContext"/>.
        /// </summary>
        void Save();
    }
}
