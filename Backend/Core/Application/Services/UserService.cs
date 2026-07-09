using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _unitOfWork.Users.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        if (id == 0) return null;
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        bool isDel = await _unitOfWork.Users.DeleteByIdAsync(id);
        if (!isDel) return false;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
