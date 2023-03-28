// Filename: IWorkoutsRepository.cs

using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Data.Repository.WorkoutsRepository
{
    /// <summary>
    /// The workouts repository interface.
    /// </summary>
    public interface IWorkoutsRepository : IDisposable
    {

        /// <summary>
        /// Gets the user dto with all workouts.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoWithAllWorkouts(string userId);

        /// <summary>
        /// Gets the user dto by workout date.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="workoutDate">The workout date.</param>
        /// <returns>A <see cref="UserDto"/>.</returns>
        UserDto GetUserDtoByWorkoutDate(string userId, string workoutDate);

        /// <summary>
        /// Gets the user workout by user workout id.
        /// This method should NOT be used in the controller to return a <see cref="UserWorkout"/> object to the client.
        /// </summary>
        /// <param name="userWorkoutId">The user workout id.</param>
        /// <returns>A <see cref="UserWorkout"/>.</returns>
        UserWorkout GetUserWorkoutByUserWorkoutId(string userWorkoutId);

        /// <summary>
        /// Adds the user workout.
        /// </summary>
        /// <param name="userWorkout">The user workout.</param>
        void AddUserWorkout(UserWorkout userWorkout);

        /// <summary>
        /// Updates the user workout.
        /// </summary>
        /// <param name="userWorkout">The user workout.</param>
        void UpdateUserWorkout(UserWorkout userWorkout);

        /// <summary>
        /// Deletes the user workout.
        /// </summary>
        /// <param name="userWorkoutId">The user workout id.</param>
        void DeleteUserWorkout(string userWorkoutId);

        /// <summary>
        /// Saves changes to the <see cref="ApplicationDbContext"/>.
        /// </summary>
        void Save();
    }
}
