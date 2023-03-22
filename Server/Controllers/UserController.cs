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
    [Authorize]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
