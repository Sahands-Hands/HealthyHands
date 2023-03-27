// Filename: UserController.cs

using HealthyHands.Server.Data;
using HealthyHands.Server.Models;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// UserController Constructor.
        /// </summary>
        /// <param name="context">HealthyHands DbContext</param>
        /// <param name="userManager">HealthyHands UserManager</param>
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var user = await _context.Users.Select(u =>
                new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Height = u.Height,
                    Gender = u.Gender,
                    ActivityLevel = u.ActivityLevel,
                    BirthDay = u.BirthDay,
                }).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if ((user == null) || (userDto.Id != user.Id))
            {
                return NotFound();
            }

            try
            {
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Height = userDto.Height;
                user.Gender = userDto.Gender;
                user.ActivityLevel = userDto.ActivityLevel;

                await _userManager.UpdateAsync(user);
            }
            catch
            {
                Response.StatusCode = 400;
                return Content("Error updating user info");
            }
            
            return Ok();
        }
    }
}
