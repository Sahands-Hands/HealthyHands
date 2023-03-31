// FileName: WeightsRepository.cs

using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.WeightsRepository
{
    /// <summary>
    /// The weights repository.
    /// </summary>
    public class WeightsRepository : IWeightsRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightsRepository"/> class.
        /// </summary>
        /// <param name="context">The db context.</param>
        public WeightsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the user dto with all weights.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public UserDto GetUserDtoWithAllWeights(string userId)
        {
            var user = _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                CalorieGoal = u.CalorieGoal,
                UserWeights = u.UserWeights
            }).FirstOrDefault(u => u.Id == userId);

            return user;
        }

        /// <summary>
        /// Gets the user dto filtered by weight date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="weightDate">The weight date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public UserDto GetUserDtoByWeightDate(string userId, string weightDate)
        {
            DateTime weightDateTime;
            DateTime.TryParse(weightDate, out weightDateTime);

            var user = _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                CalorieGoal = u.CalorieGoal,
                UserWeights = u.UserWeights
            }).FirstOrDefault(u => u.Id == userId);

            user.UserWeights = user.UserWeights.Where(u => u.WeightDate.Date == weightDateTime.Date).Select(w => w).ToList();

            return user;
        }

        /// <summary>
        /// Gets the user weight by user weight id.
        /// </summary>
        /// <param name="userWeightId">The user weight id.</param>
        /// <returns>A <see cref="UserWeight"/>.</returns>
        public UserWeight GetUserWeightByUserWeightId(string userWeightId)
        {
            var weight = _context.UserWeights.Select(w => w).FirstOrDefault(w => w.UserWeightId == userWeightId);
            return weight;
        }

        /// <summary>
        /// Adds the user weight.
        /// </summary>
        /// <param name="userWeight">The user weight.</param>
        public void AddUserWeight(UserWeight userWeight)
        {
            _context.UserWeights.AddAsync(userWeight);
        }

        /// <summary>
        /// Updates the user weight.
        /// </summary>
        /// <param name="userWeight">The user weight.</param>
        public void UpdateUserWeight(UserWeight userWeight)
        {
            var weightToUpdate = _context.UserWeights.Select(w => w).FirstOrDefault(w => w.UserWeightId == userWeight.UserWeightId && w.ApplicationUserId == userWeight.ApplicationUserId);

            weightToUpdate.Weight = userWeight.Weight;
            weightToUpdate.WeightDate = userWeight.WeightDate;

            _context.Update(weightToUpdate);
        }

        /// <summary>
        /// Deletes the user weight.
        /// </summary>
        /// <param name="userWeightId">The user weight id.</param>
        public void DeleteUserWeight(string userWeightId)
        {
            var userWeightToRemove = _context.UserWeights.Select(w => w)
                .FirstOrDefault(w => w.UserWeightId == userWeightId);
            if (userWeightToRemove != null)
            {
                _context.UserWeights.Remove(userWeightToRemove);
            }
        }

        /// <summary>
        /// Saves changes made to the <see cref="ApplicationDbContext"/>.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
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
