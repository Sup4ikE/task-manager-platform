using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Data;

namespace Infrastructure.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly TaskManagerContext _context;
    public ITaskRepository Tasks { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(TaskManagerContext context, ITaskRepository tasks, IUserRepository users)
    {
        _context = context;
        Tasks = tasks;
        Users = users;
    }
    
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}