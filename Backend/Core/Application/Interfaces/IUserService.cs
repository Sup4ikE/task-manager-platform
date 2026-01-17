using TaskManager_API.Contracts.DTOs;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDTO>> GetAllAsync();
    Task<UserResponseDTO?> GetByIdAsync(int id);
    Task<bool> DeleteUserAsync(int id);
}