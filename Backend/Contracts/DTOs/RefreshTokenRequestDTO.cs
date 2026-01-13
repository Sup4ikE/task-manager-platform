namespace TaskManager_API.Contracts.DTOs;

public class RefreshTokenRequestDTO
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public required string RefreshToken { get; set; }
}