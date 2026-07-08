using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync(int userId);
    Task<TaskItem?> GetByIdAsync(int userId, int taskId);
    Task<TaskItem> AddAsync(TaskItem taskItem, int userId);
    Task<bool> UpdateAsync(int userId, int taskId, TaskItem taskItem);
    Task<bool> DeleteByIdAsync(int userId, int taskId);
}