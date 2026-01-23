namespace TaskManager_API.Contracts.DTOs;

public class RefreshTokenRequestDTO
{
    public int UserId { get; set; }
    public required string RefreshToken { get; set; }
}