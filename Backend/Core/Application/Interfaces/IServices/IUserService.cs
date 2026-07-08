using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> DeleteUserAsync(int id);
}
