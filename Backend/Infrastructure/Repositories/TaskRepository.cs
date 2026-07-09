using Microsoft.EntityFrameworkCore;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;
using TaskManager_API.Data;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerContext _context;

    public TaskRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllAsync(int userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return tasks;
    }

    public async Task<TaskItem?> GetByIdAsync(int userId, int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        
        return task;
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem)
    {
        if (taskItem == null) throw new ArgumentNullException(nameof(taskItem));
        
        await _context.Tasks.AddAsync(taskItem);

        return taskItem;
    }

    public async Task<bool> DeleteByIdAsync(int userId, int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        
        if (task == null) return false;
        
        _context.Tasks.Remove(task);
        
        return true;
    }
}