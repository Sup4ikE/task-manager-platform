using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;

namespace TaskManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDTO request)
    {
        var user = await _authService.RegisterAsync(request.Username, request.Password);
        if (user == null) return BadRequest("Username already exists");

        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] UserDTO request)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password);
        if (result == null) return BadRequest("Error in login");

        return Ok(new TokenResponseDTO
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var success = await _authService.LogoutAsync(userId);
        if (!success) return NotFound("User not found");

        return Ok("Logged out successfully");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO request)
    {
        var result = await _authService.RefreshTokensAsync(request.UserId, request.RefreshToken);
        if (result is null) return Unauthorized("Invalid refresh token.");

        return Ok(new TokenResponseDTO
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        });
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
