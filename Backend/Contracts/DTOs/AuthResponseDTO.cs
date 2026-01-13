namespace TaskManager_API.Contracts.DTOs;

public class AuthResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    // public User User { get; set; } = null!;
}