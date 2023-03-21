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
    public class UserWeightsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserWeightsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<UserWeight>>> GetAllUserWeights()
        {
            var user = await _context.Users.Include(u => u.UserWeights).Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserWeights = u.UserWeights
            }).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("api/weights/add")]
        public async Task<ActionResult> AddUserWeight([FromBody] UserWeight userWeight)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
            {
                return NotFound();
            }

            //_context.UserWeights.Add(userWeight);
            //_context.SaveChanges();

            return Ok();
        }
    }
}
