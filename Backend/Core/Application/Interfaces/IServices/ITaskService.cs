using TaskManager_API.Contracts.DTOs;

namespace TaskManager_API.Core.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskDTO>> GetAllAsync(int userId);
    Task<TaskDTO?> GetByIdAsync(int userId, int taskId);
    Task<TaskDTO> AddAsync(TaskDTO taskItem, int userId);
    Task<bool> UpdateAsync(int userId, int taskId, TaskDTO taskItem);
    Task<bool> DeleteByIdAsync(int userId, int taskId);
}