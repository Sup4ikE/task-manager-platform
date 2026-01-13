using System.Security.Claims;
using System.Xml.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using Shared.DTO;
using Shared.Models;
using TaskManager_API.Services;

namespace TaskManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService): Controller
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody]UserDTO request)
    {
        var user = await authService.RegisterAsync(request);
        if (user == null)
        {
            return BadRequest("User not found");
        }
        
        return Ok(user);
    }


    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(UserDTO request)
    {
        var result = await authService.LoginAsync(request);
        if (result == null)
        {
            return BadRequest("Error in login");
        }
        
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var success = await authService.LogoutAsync(userId);
        if (!success)
        {
            return NotFound("User not found");
        }

        return Ok("Logged out successfully");
    }

    
    [HttpPost("refresh-token")]
    public async Task<ActionResult<User>> RefreshToken(RefreshTokenRequestDTO request)
    {
        var result = await authService.RefreshTokensAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
        {
            return Unauthorized("Invalid refresh token.");
        }

        return Ok(result);
    }

     [Authorize]
     [HttpGet]
     public IActionResult AuthenticatedOnlyEndpoint()
     {
         return Ok("You are authenticated!");
     }

     [Authorize(Roles = "Admin")]
     [HttpGet("admin-only")]
     public IActionResult AdminOnlyEndpoint()
     {
         return Ok("You are admin!");
     }
}

