using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

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
        
        var dtos = tasks.Select(x => new TaskDTO() 
        {  
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            IsCompleted = x.IsCompleted,
            Created = x.Created 
        }).ToList();
        
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDTO>> GetByIdAsync(int id)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        
        var task = await _taskService.GetByIdAsync(userId, id);

        if (task == null) return NotFound();
        
        var taskDto = new TaskDTO()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Created = task.Created
        };
        
        return Ok(taskDto);
    }

    [HttpPost]
    //  "2025-08-14T00:00:00Z"
    public async Task<ActionResult<TaskDTO>> AddAsync(TaskDTO taskItem)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();;

        var task = new TaskItem()
        {
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted
        };
        
        var newTask = await _taskService.AddAsync(task, userId);

        var newTaskDto = new TaskDTO()
        {
            Id = newTask.Id,
            Title = newTask.Title,
            Description = newTask.Description,
            IsCompleted = newTask.IsCompleted,
            Created = newTask.Created
        };
        
        return CreatedAtAction(nameof(GetByIdAsync), new {id = newTaskDto.Id}, newTaskDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(int id, TaskDTO taskItem)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var task = new TaskItem()
        {
            Id = id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted
        };
        
        var isUpdated = await _taskService.UpdateAsync(userId, id, task);

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