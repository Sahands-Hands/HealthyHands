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
        catch (Exception e)
        {
            Console.WriteLine("Here is the exception: {0}", e.ToString());
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
    public async Task<ActionResult> LockoutUser([FromBody] string userId)
    {
        if (!await _adminRepository.UserExists(userId))
        {
            return NotFound();
        }

        try
        {
            await _adminRepository.LockoutUser(userId);
            await _adminRepository.Save();
        }
        catch
        {
            return BadRequest();
        }
        
        return Ok();
    }
    
    [HttpPut]
    [Route("unlock")]
    public async Task<ActionResult>UnlockUser([FromBody] string userId)
    {
        if (!await _adminRepository.UserExists(userId))
        {
            return NotFound();
        }

        try
        {
            await _adminRepository.UnlockUser(userId);
            await _adminRepository.Save();
        }
        catch
        {
            return BadRequest();
        }
        
        return Ok();
    }

    [HttpPut]
    [Route("role/admin")]
    public async Task<ActionResult> SetUserRoleAdmin([FromBody] string userId)
    {
        if (!await _adminRepository.UserExists(userId))
        {
            return NotFound();
        }

        try
        {
            await _adminRepository.ChangeUserRoleToAdmin(userId);
            await _adminRepository.Save();
        }
        catch
        {
            return BadRequest();
        }
        
        return Ok();
    }

    [HttpPut]
    [Route("role/user")]
    public async Task<ActionResult> SetUserRoleUser([FromBody] string userId)
    {
        if (!await _adminRepository.UserExists(userId))
        {
            return NotFound();
        }

        try
        {
            await _adminRepository.ChangeUserRoleToUser(userId);
            await _adminRepository.Save();
        }
        catch
        {
            return BadRequest();
        }
        
        return Ok();
    }

    [HttpPut]
    [Route("reset")]
    public async Task<ActionResult> ResetUserPassword([FromBody] string userId)
    {
        if (!await _adminRepository.UserExists(userId))
        {
            return NotFound();
        }

        try
        {
            await _adminRepository.ResetUserPassword(userId);
            await _adminRepository.Save();
        }
        catch
        {
            return BadRequest();
        }
        
        return Ok();
    }
}