using Microsoft.AspNetCore.Identity;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Application.Models;
using TaskManager_API.Core.Domain;

namespace TaskManager_API.Core.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenService = refreshTokenService;
    }

    private async Task<AuthResult> CreateTokenResponse(User user)
    {
        return new AuthResult
        {
            AccessToken = _jwtTokenGenerator.CreateToken(user),
            RefreshToken = await _refreshTokenService.GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    public async Task<User?> RegisterAsync(string username, string password)
    {
        var existing = await _userRepository.GetByUsernameAsync(username);
        if (existing != null) return null;

        var user = new User
        {
            Username = username,
            Role = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task<AuthResult?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null) return null;

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verify == PasswordVerificationResult.Failed) return null;

        return await CreateTokenResponse(user);
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<AuthResult?> RefreshTokensAsync(int userId, string refreshToken)
    {
        var user = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);
        if (user is null) return null;

        return await CreateTokenResponse(user);
    }
}
