using Microsoft.EntityFrameworkCore;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;
using TaskManager_API.Data;

namespace Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly TaskManagerContext _context;

    public UserRepository(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        
        return users;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;
        
        return user;
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        
        return user;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return false;
        
        _context.Users.Remove(user);
        
        return true;
    }
}