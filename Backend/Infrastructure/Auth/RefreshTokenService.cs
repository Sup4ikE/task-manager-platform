using System.Security.Cryptography;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Infrastructure.Auth;

public class RefreshTokenService: IRefreshTokenService
{
    private readonly IUserRepository _userRepository;
    
    public RefreshTokenService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    
    public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.SaveChangesAsync();
        return refreshToken;
    }
    
    public async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) return null;

        if (string.IsNullOrWhiteSpace(user.RefreshToken)) return null;
        if (user.RefreshTokenExpiryTime is null) return null;
        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow) return null;

        if (user.RefreshToken != refreshToken) return null;

        return user;
    }
}