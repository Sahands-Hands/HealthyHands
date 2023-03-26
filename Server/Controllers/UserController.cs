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
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return Ok();
        }

        //[HttpGet]
        //public async Task<ActionResult> Get(string id)
        //{
        //    var user = await _userManager.FindByNameAsync(id);
        //    return View(user);
        //}

        [HttpGet]
        [Route("UserInfo")]
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

            Console.WriteLine("UserDto object: {0}", user);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
