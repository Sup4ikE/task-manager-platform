using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using Shared.Models;
using TaskManager_API.Data;

namespace TaskManager_API.Services;

public class AuthService(Context context, IConfiguration configuration): IAuthService
{
    // Registation
    public async Task<User?> RegisterAsync(UserDTO request)
    {
        if (await context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return null;
        }

        var user = new User();
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
   
        user.Username = request.Username;
        user.PasswordHash = hashedPassword;
    
        context.Users.Add(user);
        await context.SaveChangesAsync();
    
        return user;
    }
    
    // Login
    public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (user == null)
        {
            return null;
        }
        
        if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return await CreateTokenResponse(user);
    }
    private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
    {
        return new TokenResponseDTO
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }
    
    
    // Logout
    public async Task<bool> LogoutAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await context.SaveChangesAsync();
        return true;
    }

    
    // Create Refresh Token
    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
     {
         var refreshToken = GenerateRefreshToken();
         user.RefreshToken = refreshToken;
         user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
         await context.SaveChangesAsync();
         return refreshToken;
     }
    private string GenerateRefreshToken()
     {
         var randomNumber = new byte[32];
         using var rng = RandomNumberGenerator.Create();
         rng.GetBytes(randomNumber);
         return Convert.ToBase64String(randomNumber);
     }
    public async Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }
    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }
    
    
    // Create JWT Token
    private string CreateToken(User user)
     {
         var claims = new List<Claim>
         {
             new Claim(ClaimTypes.Name, user.Username),
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Role, user.Role)
         };

         var key = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

         var tokenDescriptor = new JwtSecurityToken(
             issuer: configuration.GetValue<string>("AppSettings:Issuer"),
             audience: configuration.GetValue<string>("AppSettings:Audience"),
             claims: claims,
             expires: DateTime.UtcNow.AddDays(1),
             signingCredentials: creds
         );

         return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
     }
}

