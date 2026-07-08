using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        if (id == 0) return null;
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        bool isDel = await _userRepository.DeleteByIdAsync(id);
        if (!isDel) return false;

        await _userRepository.SaveChangesAsync();
        return true;
    }
}
