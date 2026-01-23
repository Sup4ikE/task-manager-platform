using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;

namespace TaskManager_API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
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

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDTO>> GetByIdAsync(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteByIdAsync(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        
        if (result == false) return NotFound();
        
        return Ok(result);
    }
}
