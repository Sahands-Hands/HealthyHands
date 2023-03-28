// FileName: WorkoutsRepository.cs

using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.WorkoutsRepository
{
    /// <summary>
    /// The workouts repository.
    /// </summary>
    public class WorkoutsRepository : IWorkoutsRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutsRepository"/> class.
        /// </summary>
        /// <param name="context">The db context.</param>
        public WorkoutsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the user dto with all workouts.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public UserDto GetUserDtoWithAllWorkouts(string userId)
        {
            var user = _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserWorkouts = u.UserWorkouts
            }).FirstOrDefault(u => u.Id == userId);

            return user;
        }

        /// <summary>
        /// Gets the user dto filtered by workout date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="workoutDate">The workout date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        public UserDto GetUserDtoByWorkoutDate(string userId, string workoutDate)
        {
            DateTime workoutDateTime;
            DateTime.TryParse(workoutDate, out workoutDateTime);

            var user = _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserWorkouts = u.UserWorkouts
            }).FirstOrDefault(u => u.Id == userId);

            user.UserWorkouts = user.UserWorkouts.Where(u => u.WorkoutDate.Date == workoutDateTime.Date).Select(w => w).ToList();

            return user;
        }

        /// <summary>
        /// Gets the user workout by user workout id.
        /// </summary>
        /// <param name="userWorkoutId">The user workout id.</param>
        /// <returns>A <see cref="UserWorkout"/>.</returns>
        public UserWorkout GetUserWorkoutByUserWorkoutId(string userWorkoutId)
        {
            var workout = _context.UserWorkouts.Select(w => w).FirstOrDefault(w => w.UserWorkoutId == userWorkoutId);
            return workout;
        }

        /// <summary>
        /// Adds the user workout.
        /// </summary>
        /// <param name="userWorkout">The user workout.</param>
        public void AddUserWorkout(UserWorkout userWorkout)
        {
            _context.UserWorkouts.AddAsync(userWorkout);
        }

        /// <summary>
        /// Updates the user workout.
        /// </summary>
        /// <param name="userWorkout">The user workout.</param>
        public void UpdateUserWorkout(UserWorkout userWorkout)
        {
            var workoutToUpdate = _context.UserWorkouts.Select(w => w).FirstOrDefault(w => w.UserWorkoutId == userWorkout.UserWorkoutId && w.ApplicationUserId == userWorkout.ApplicationUserId);
            
            workoutToUpdate.WorkoutName = userWorkout.WorkoutName;
            workoutToUpdate.WorkoutType = userWorkout.WorkoutType;
            workoutToUpdate.Intensity = userWorkout.Intensity;
            workoutToUpdate.Length = userWorkout.Length;
            workoutToUpdate.WorkoutDate = userWorkout.WorkoutDate;
            workoutToUpdate.CaloriesBurned = userWorkout.CaloriesBurned;

            _context.Update(workoutToUpdate);
        }

        /// <summary>
        /// Deletes the user workout.
        /// </summary>
        /// <param name="userWorkoutId">The user workout id.</param>
        public void DeleteUserWorkout(string userWorkoutId)
        {
            var userWorkoutToRemove = _context.UserWorkouts.Select(w => w)
                .FirstOrDefault(w => w.UserWorkoutId == userWorkoutId);
            if (userWorkoutToRemove != null)
            {
                _context.UserWorkouts.Remove(userWorkoutToRemove);
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
