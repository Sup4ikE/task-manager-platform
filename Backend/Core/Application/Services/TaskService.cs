using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Services;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskItem>> GetAllAsync(int userId)
    {
        if (userId <= 0) return new List<TaskItem>();
        
        var list = await _taskRepository.GetAllAsync(userId);
        
        return list;
    }

    public async Task<TaskItem?> GetByIdAsync(int userId, int taskId)
    {
        if (userId <= 0 || taskId <= 0) return null;

        var taskItem = await _taskRepository.GetByIdAsync(userId, taskId);

        if (taskItem == null) return null;
        
        return taskItem;
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem, int userId)
    {
        if (userId <= 0) throw new ArgumentException("Invalid user id", nameof(userId));

        taskItem.UserId = userId;
        taskItem.Created = DateTime.UtcNow;

        await _taskRepository.AddAsync(taskItem);
        await _taskRepository.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> UpdateAsync(int userId, int taskId, TaskItem taskItem)
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