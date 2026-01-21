using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task<bool> DeleteByIdAsync(int id);
    Task SaveChangesAsync();
}