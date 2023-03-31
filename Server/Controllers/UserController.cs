// Filename: UserController.cs

using HealthyHands.Server.Data;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HealthyHands.Server.Data.Repository.UserRepository;

namespace HealthyHands.Server.Controllers
{
    /// <summary>
    /// This is the UserInfo Controller. This controller includes methods for accessing and updating user info.
    /// The default controller route is "user"
    /// </summary>
    [Authorize]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// UserController Constructor.
        /// </summary>
        /// <param name="userRepository"> <see cref="UserRepository"/></param>
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// This method returns the user information for the logged in user.
        /// The Http route is "user"
        /// </summary>
        /// <returns>A <see cref="HealthyHands.Shared.Models.UserDto"/> with the user's info and an <see cref="OkResult"/>.</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<UserDto>> UserInfo()
        {
            var userDto = new UserDto();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                userDto = await _userRepository.GetUser(userId);
            }
            catch
            {
                return BadRequest(userDto);
            }

            if (userDto == null)
            {
                return NotFound(userDto);
            }

            return Ok(userDto);
        }

        /// <summary>
        /// This method accepts a UserDto object from the client and updates the user in the database found by Id.
        /// The Http route is "user/update".
        /// </summary>
        /// <param name="userDto"> A <see cref="HealthyHands.Shared.Models.UserDto"/> from Client with properties to be updated.</param>
        /// <returns><see cref="OkResult"/> upon success, otherwise <see cref="ContentResult"/> with <see cref="NotFoundObjectResult"/> and error description</returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateUserInfo([FromBody] UserDto userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != userDto.Id)
            {
                return Forbid();
            }

            try
            {
                await _userRepository.UpdateUser(userDto, userId);
                _userRepository.Save();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
