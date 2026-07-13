using TaskManager_API.Core.Domain;

namespace Core.Application.Specifications;

public class IncompleteTasksSpec : Specification<TaskItem>
{
    public IncompleteTasksSpec(int userId)
    {
        AddCriteria(t => t.UserId == userId && !t.IsCompleted);
        ApplyOrderByDescending(t => t.Created);
    }
}