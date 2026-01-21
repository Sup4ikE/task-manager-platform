using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<string> GenerateAndSaveRefreshTokenAsync(User user);
    Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken);
}