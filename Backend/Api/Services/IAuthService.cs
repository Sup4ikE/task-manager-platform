using Shared.Models;
using Shared.DTO;

namespace TaskManager_API.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDTO request);
    Task<TokenResponseDTO?> LoginAsync(UserDTO request);
    Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request);
    Task<bool> LogoutAsync(int userId);
}