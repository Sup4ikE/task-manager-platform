namespace TaskManager_API.Core.Domain;

public class TaskItem
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    
    public int UserId { get; set; }
    public User? User { get; set; }
}