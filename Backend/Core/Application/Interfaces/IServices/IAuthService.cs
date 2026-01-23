using TaskManager_API.Core.Domain;
using TaskManager_API.Contracts.DTOs;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDTO request);
    Task<TokenResponseDTO?> LoginAsync(UserDTO request);
    Task<bool> LogoutAsync(int userId);
    Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request);
}