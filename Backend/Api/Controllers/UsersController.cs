using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;

namespace TaskManager_API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDTO>>> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();

        var result = users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDTO>> GetByIdAsync(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteByIdAsync(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();

        return Ok(result);
    }
}
