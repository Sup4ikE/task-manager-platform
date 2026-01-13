namespace TaskManager_API.Contracts.DTOs;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}