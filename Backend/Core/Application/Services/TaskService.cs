using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Domain;

namespace Core.Application.Services;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskDTO>> GetAllAsync(int userId)
    {
        if (userId <= 0) return new List<TaskDTO>();
        
        var list = await _taskRepository.GetAllAsync(userId);
        
        var ResultTasks = list
        .Select(t => new TaskDTO
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            Created = t.Created
        })
        .ToList();
        
        return ResultTasks;
    }

    public async Task<TaskDTO?> GetByIdAsync(int userId, int taskId)
    {
        if (userId <= 0 || taskId <= 0) return null;

        var taskItem = await _taskRepository.GetByIdAsync(userId, taskId);

        if (taskItem == null) return null;
        
        var taskDto = new TaskDTO()
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted,
            Created = taskItem.Created
        };
        
        return taskDto;
    }

    public async Task<TaskDTO> AddAsync(TaskDTO taskItem, int userId)
    {
        if (userId <= 0) throw new ArgumentException("Invalid user id", nameof(userId));
        
        var newTask = new TaskItem
        {
            Title = taskItem.Title,
            Description = taskItem.Description,
            Created = DateTime.UtcNow,
            IsCompleted = taskItem.IsCompleted,
            UserId = userId
        };
        
        await _taskRepository.AddAsync(newTask);
        await _taskRepository.SaveChangesAsync();

        var newTaskDto = new TaskDTO()
        {
            Id = newTask.Id,
            Title = newTask.Title,
            Description = newTask.Description,
            IsCompleted = newTask.IsCompleted,
            Created = newTask.Created
        };
        
        return newTaskDto;
    }

    public async Task<bool> UpdateAsync(int userId, int taskId, TaskDTO taskItem)
    {
        if (userId <= 0 || taskId <= 0)
            return false;

        var task = await _taskRepository.GetByIdAsync(userId, taskId);
        if (task == null)
            return false;

        task.Title = taskItem.Title;
        task.Description = taskItem.Description;
        task.IsCompleted = taskItem.IsCompleted;

        await _taskRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteByIdAsync(int userId, int taskId)
    {
        var isDel = await _taskRepository.DeleteByIdAsync(userId, taskId);
        
        if (isDel == false) return false;
        
        await _taskRepository.SaveChangesAsync();
        
        return true;
    }
}