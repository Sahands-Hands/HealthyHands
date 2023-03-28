// Filename: IWeightsRepository.cs

using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.WeightsRepository
{
    /// <summary>
    /// The weights repository interface.
    /// </summary>
    public interface IWeightsRepository : IDisposable
    {

        /// <summary>
        /// Gets the user dto with all weights.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoWithAllWeights(string userId);

        /// <summary>
        /// Gets the user dto by weight date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="weightDate">The weight date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoByWeightDate(string userId, string weightDate);

        /// <summary>
        /// Gets the user weight by user weight id.
        /// This method should NOT be used in the controller to return a <see cref="UserWeight"/> object to the client.
        /// </summary>
        /// <param name="userWeightId">The user weight id.</param>
        /// <returns>A <see cref="UserWeight"/>.</returns>
        UserWeight GetUserWeightByUserWeightId(string userWeightId);

        /// <summary>
        /// Adds the user weight.
        /// </summary>
        /// <param name="userWeight">The user weight.</param>
        void AddUserWeight(UserWeight userWeight);

        /// <summary>
        /// Updates the user weight.
        /// </summary>
        /// <param name="userWeight">The user weight.</param>
        void UpdateUserWeight(UserWeight userWeight);

        /// <summary>
        /// Deletes the user weight.
        /// </summary>
        /// <param name="userWeightId">The user weight id.</param>
        void DeleteUserWeight(string userWeightId);

        /// <summary>
        /// Saves changes to the <see cref="ApplicationDbContext"/>.
        /// </summary>
        void Save();
    }
}
