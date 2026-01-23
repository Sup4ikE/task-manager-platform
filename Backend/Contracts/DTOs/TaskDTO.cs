namespace TaskManager_API.Contracts.DTOs;

public class TaskDTO
{
    public int Id { get; set; } 
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
}