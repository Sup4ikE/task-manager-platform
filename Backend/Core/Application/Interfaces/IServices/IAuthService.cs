using TaskManager_API.Core.Application.Models;
using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(string username, string password);
    Task<AuthResult?> LoginAsync(string username, string password);
    Task<bool> LogoutAsync(int userId);
    Task<AuthResult?> RefreshTokensAsync(int userId, string refreshToken);
}
