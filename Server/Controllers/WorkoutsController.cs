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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutsRepository _workoutsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutsController"/> class.
        /// </summary>
        /// <param name="workoutsRepository">The workouts repository.</param>
        public WorkoutsController(IWorkoutsRepository workoutsRepository, UserManager<ApplicationUser> userManager)
        {
            _workoutsRepository = workoutsRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all workouts for logged in user.
        /// </summary>
        /// <returns>A <see cref="UserDto"/> with workouts</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<UserDto>> GetWorkouts()
        {
            UserDto user = new UserDto();
            
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = _userManager.GetUserAsync(User).Result.Id;
            try
            {
                user = await _workoutsRepository.GetUserDtoWithAllWorkouts(userId);
            }
            catch
            {
                return BadRequest(user);
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets all workouts for logged in user filtered by workout date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="UserDto"/> with workouts</returns>
        [HttpGet]
        [Route("byDate/{date}")]
        public async Task<ActionResult<UserDto>> GetByWorkoutDate(string date)
        {
            UserDto? user;
            var userId = _userManager.GetUserAsync(User).Result.Id;

            try
            {
                user = await _workoutsRepository.GetUserDtoByWorkoutDate(userId, date);
            }
            catch
            {
                return BadRequest();
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
            var userWorkout = new UserWorkout
            {
                UserWorkoutId = Guid.NewGuid().ToString(),
                WorkoutName = userWorkoutDto.WorkoutName,
                WorkoutType = userWorkoutDto.WorkoutType,
                Intensity = userWorkoutDto.Intensity,
                Length = userWorkoutDto.Length,
                WorkoutDate = userWorkoutDto.WorkoutDate,
                CaloriesBurned = userWorkoutDto.CaloriesBurned,
                ApplicationUserId = _userManager.GetUserAsync(User).Result.Id
            };

            try
            {
                await _workoutsRepository.AddUserWorkout(userWorkout);
                await _workoutsRepository.Save();
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
            var userId = _userManager.GetUserAsync(User).Result.Id;
            if (workoutDto.ApplicationUserId != userId)
            {
                return NotFound();
            }

            UserWorkout workout = new UserWorkout
            {
                UserWorkoutId = workoutDto.UserWorkoutId,
                WorkoutName = workoutDto.WorkoutName,
                WorkoutType = workoutDto.WorkoutType,
                Intensity = workoutDto.Intensity,
                Length = workoutDto.Length,
                WorkoutDate = workoutDto.WorkoutDate,
                CaloriesBurned = workoutDto.CaloriesBurned,
                ApplicationUserId = userId    // Use the current user's Id!!!
            };

            try
            {
                await _workoutsRepository.UpdateUserWorkout(workout);
                await _workoutsRepository.Save();
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
        [Route("delete/{userWorkoutId}")]
        public async Task<ActionResult> DeleteWorkout(string userWorkoutId)
        {
            UserWorkout workoutToDelete = await _workoutsRepository.GetUserWorkoutByUserWorkoutId(userWorkoutId);
            if (workoutToDelete == null || workoutToDelete.ApplicationUserId != _userManager.GetUserAsync(User).Result.Id)
            {
                return NotFound();
            }

            try
            { 
                await _workoutsRepository.DeleteUserWorkout(userWorkoutId);
                await _workoutsRepository.Save();
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
