// Filename: WeightsController.cs

using System.Security.Claims;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WeightsRepository;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyHands.Server.Controllers
{
    /// <summary>
    /// The weights controller.
    /// </summary>
    [Authorize]
    [Route("weights")]
    public class WeightsController : Controller
    {
        private readonly IWeightsRepository _weightsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightsController"/> class.
        /// </summary>
        /// <param name="weightsRepository">The weights repository.</param>
        public WeightsController(IWeightsRepository weightsRepository)
        {
            _weightsRepository = weightsRepository;
        }

        /// <summary>
        /// Gets all weights for logged in user.
        /// </summary>
        /// <returns>A <see cref="UserDto"/> with weights</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<UserDto>> GetWeights()
        {
            UserDto? user;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                user = await _weightsRepository.GetUserDtoWithAllWeights(userId);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets all weights for logged in user filtered by weight date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="UserDto"/> with weights</returns>
        [HttpGet]
        [Route("byDate")]
        public async Task<ActionResult<UserDto>> GetByWeightDate(string date)
        {
            UserDto? user;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                user = await _weightsRepository.GetUserDtoByWeightDate(userId, date);
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
        /// Adds the weight.
        /// </summary>
        /// <param name="userWeightDto">The user weight dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("add")]
        public async Task<ActionResult> AddWeight([FromBody] UserWeightDto userWeightDto)
        {
            UserWeight userWeight = new UserWeight
            {
                UserWeightId = Guid.NewGuid().ToString(),
                Weight = userWeightDto.Weight,
                WeightDate = userWeightDto.WeightDate,
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            try
            {
                await _weightsRepository.AddUserWeight(userWeight);
                await _weightsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Updates the weight.
        /// </summary>
        /// <param name="weightDto">The weight dto.</param>
        /// <returns>An Http Status Code</returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateWeight([FromBody] UserWeightDto weightDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (weightDto.ApplicationUserId != userId)
            {
                return NotFound();
            }

            UserWeight weight = new UserWeight
            {
                UserWeightId = weightDto.UserWeightId,
                Weight = weightDto.Weight,
                WeightDate = weightDto.WeightDate,
                ApplicationUserId = userId    // Use the current user's Id!!!
            };

            try
            {
                await _weightsRepository.UpdateUserWeight(weight);
                await _weightsRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes the weight.
        /// </summary>
        /// <param name="userWeightId">The user weight id.</param>
        /// <returns>An Http Status Code</returns>
        [HttpDelete]
        [Route("delete/{userWeightId}")]
        public async Task<ActionResult> DeleteWeight(string userWeightId)
        {
            UserWeight weightToDelete = _weightsRepository.GetUserWeightByUserWeightId(userWeightId);
            if (weightToDelete == null || weightToDelete.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            try
            {
                await _weightsRepository.DeleteUserWeight(userWeightId);
                await _weightsRepository.Save();
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
            _weightsRepository.Dispose();
            base.Dispose(disposing);
        }
    }


}
