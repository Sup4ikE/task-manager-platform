using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;

namespace TaskManager_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController: ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private bool TryGetUserId(out int userId)
    {
        userId = default;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return false;

        return int.TryParse(userIdClaim.Value, out userId);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TaskDTO>>> GetAllAsync()
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        
        var tasks = await _taskService.GetAllAsync(userId);
        
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDTO>> GetByIdAsync(int id)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        
        var task = await _taskService.GetByIdAsync(userId, id);
        
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPost]
    //  "2025-08-14T00:00:00Z"
    public async Task<ActionResult<TaskDTO>> AddAsync(TaskDTO taskItem)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();;

        var newTask = await _taskService.AddAsync(taskItem, userId);
        
        return CreatedAtAction(nameof(GetByIdAsync), new {id = newTask.Id}, newTask);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, TaskDTO taskItem)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var isUpdated = await _taskService.UpdateAsync(userId, id, taskItem);

        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteByIdAsync(int id)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        
        var isDeleted = await _taskService.DeleteByIdAsync(userId, id);
        
        return isDeleted ? NoContent() : NotFound();
    }
}