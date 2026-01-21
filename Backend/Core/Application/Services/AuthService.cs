using Microsoft.AspNetCore.Identity;
using TaskManager_API.Core.Domain;
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Application.Interfaces;

namespace TaskManager_API.Core.Application.Services;

public class AuthService: IAuthService
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
    
    private async Task<TokenResponseDTO> CreateTokenResponse(User user)
    {
        return new TokenResponseDTO
        {
            AccessToken = _jwtTokenGenerator.CreateToken(user),
            RefreshToken = await _refreshTokenService.GenerateAndSaveRefreshTokenAsync(user)
        };
    }
    
    
    public async Task<User?> RegisterAsync(UserDTO request)
    {
        var userN = await _userRepository.GetByUsernameAsync(request.Username);
        if (userN != null) return null;

        var user = new User
        {
            Username = request.Username,
            Role = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }
    
    public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            return null;
        }
        
        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed)
        {
            return null;
        }

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
    
    public async Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request)
    {
        var user = await _refreshTokenService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }
}

