// Filename: WorkoutsController.cs

using System.Security.Claims;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WorkoutsRepository;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Controllers
{
    /// <summary>
    /// The workouts controller.
    /// </summary>
    [Authorize]
    [Route("workouts")]
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutsRepository _workoutsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutsController"/> class.
        /// </summary>
        /// <param name="workoutsRepository">The workouts repository.</param>
        public WorkoutsController(IWorkoutsRepository workoutsRepository)
        {
            _workoutsRepository =workoutsRepository;
        }

        /// <summary>
        /// Gets all workouts for logged in user.
        /// </summary>
        /// <returns>A <see cref="UserDto"/> with workouts</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<UserDto>> GetWorkouts()
        {
            UserDto? user;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                user = _workoutsRepository.GetUserDtoWithAllWorkouts(userId);
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
        /// Gets all workouts for logged in user filtered by workout date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="UserDto"/> with workouts</returns>
        [HttpGet]
        [Route("byDate")]
        public async Task<ActionResult<UserDto>> GetByWorkoutDate(string date)
        {
            UserDto? user;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                user = _workoutsRepository.GetUserDtoByWorkoutDate(userId, date);
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
        /// Adds the workout.
        /// </summary>
        /// <param name="userWorkoutDto">The user workout dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("add")]
        public async Task<ActionResult> AddWorkout([FromBody] UserWorkoutDto userWorkoutDto)
        {
            UserWorkout userWorkout = new UserWorkout
            {
                UserWorkoutId = Guid.NewGuid().ToString(),
                WorkoutName = userWorkoutDto.WorkoutName,
                WorkoutType = userWorkoutDto.WorkoutType,
                Intensity = userWorkoutDto.Intensity,
                Length = userWorkoutDto.Length,
                WorkoutDate = userWorkoutDto.WorkoutDate,
                CaloriesBurned = userWorkoutDto.CaloriesBurned,
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            try
            {
                _workoutsRepository.AddUserWorkout(userWorkout);
                _workoutsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Updates the workout.
        /// </summary>
        /// <param name="workoutDto">The workout dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateWorkout([FromBody] UserWorkoutDto workoutDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (workoutDto.ApplicationUserId != userId)
            {
                return NotFound();
            }

            UserWorkout workout = new UserWorkout
            {
                UserWorkoutId = workoutDto.UserWorkoutId,
                WorkoutName = workoutDto.WorkoutName,
                Intensity = workoutDto.Intensity,
                Length = workoutDto.Length,
                WorkoutDate = workoutDto.WorkoutDate,
                CaloriesBurned = workoutDto.CaloriesBurned,
                ApplicationUserId = userId    // Use the current user's Id!!!
            };

            try
            {
                _workoutsRepository.UpdateUserWorkout(workout);
                _workoutsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes the workout.
        /// </summary>
        /// <param name="userWorkoutId">The user workout id.</param>
        /// <returns>An Http Status Code</returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult> DeleteWorkout(string userWorkoutId)
        {
            UserWorkout workoutToDelete = _workoutsRepository.GetUserWorkoutByUserWorkoutId(userWorkoutId);
            if (workoutToDelete == null || workoutToDelete.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            try
            { 
                _workoutsRepository.DeleteUserWorkout(userWorkoutId);
                _workoutsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Disposes the <see cref="WorkoutsRepository"/>.
        /// </summary>
        /// <param name="disposing">If true, disposing.</param>
        protected override void Dispose(bool disposing)
        {
            _workoutsRepository.Dispose();
            base.Dispose(disposing);
        }
    }

    
}
