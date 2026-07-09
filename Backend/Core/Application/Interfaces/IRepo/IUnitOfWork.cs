namespace TaskManager_API.Core.Application.Interfaces;

public interface IUnitOfWork
{
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}