using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Contracts.DTOs;

namespace Core.Application.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<List<UserResponseDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        
        var usersDto =  users.Select(user => new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        }).ToList();

        return usersDto;
    }
    
    public async Task<UserResponseDTO?> GetByIdAsync(int id)
    {
        if (id == 0) return null;
        
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        var dto = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
        
        return dto;
    }
    
    public async Task<bool> DeleteUserAsync(int id)
    {
        bool isDel = await _userRepository.DeleteByIdAsync(id);
        
        if(isDel == false) return false;
        
        await _userRepository.SaveChangesAsync();

        return true;
    }
}