using System.ComponentModel.DataAnnotations;

namespace TaskManager_API.Contracts.DTOs;

public class UserDTO
{
    public int Id { get; set; } 
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    
}