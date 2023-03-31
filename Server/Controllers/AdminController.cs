using HealthyHands.Server.Data.Repository.AdminRepository;
using HealthyHands.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyHands.Server.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;

    public AdminController(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        List<UserDto>? userDtoList = null;
        try
        {
            userDtoList = await _adminRepository.GetAllUsers();
        }
        catch
        {
            return BadRequest(userDtoList);
        }

        if (userDtoList.Count == 0)
        {
            return NotFound(userDtoList);
        }

        return Ok(userDtoList);
    }
    
    [HttpPut]
    [Route("lockout")]
    public async Task<ActionResult> LockoutUser(string userId)
    {
        return Ok();
    }

    [HttpPut]
    [Route("role/admin")]
    public async Task<ActionResult> SetUserRoleAdmin(string userId)
    {
        return Ok();
    }

    [HttpPut]
    [Route("role/user")]
    public async Task<ActionResult> SetUserRoleUser(string userId)
    {
        return Ok();
    }

    [HttpPut]
    [Route("pwdrst")]
    public async Task<ActionResult> ResetUserPassword(string userId)
    {
        return Ok();
    }
}