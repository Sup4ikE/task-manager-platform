using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Services;

public class TaskService: ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TaskItem>> GetAllAsync(int userId)
    {
        if (userId <= 0) return new List<TaskItem>();
        
        var list = await _unitOfWork.Tasks.GetAllAsync(userId);
        
        return list;
    }

    public async Task<TaskItem?> GetByIdAsync(int userId, int taskId)
    {
        if (userId <= 0 || taskId <= 0) return null;

        var taskItem = await _unitOfWork.Tasks.GetByIdAsync(userId, taskId);

        if (taskItem == null) return null;
        
        return taskItem;
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem, int userId)
    {
        if (userId <= 0) throw new ArgumentException("Invalid user id", nameof(userId));

        taskItem.UserId = userId;
        taskItem.Created = DateTime.UtcNow;

        await _unitOfWork.Tasks.AddAsync(taskItem);
        await _unitOfWork.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> UpdateAsync(int userId, int taskId, TaskItem taskItem)
    {
        if (userId <= 0 || taskId <= 0)
            return false;

        var task = await _unitOfWork.Tasks.GetByIdAsync(userId, taskId);
        if (task == null)
            return false;

        task.Title = taskItem.Title;
        task.Description = taskItem.Description;
        task.IsCompleted = taskItem.IsCompleted;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteByIdAsync(int userId, int taskId)
    {
        var isDel = await _unitOfWork.Tasks.DeleteByIdAsync(userId, taskId);
        
        if (isDel == false) return false;
        
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}