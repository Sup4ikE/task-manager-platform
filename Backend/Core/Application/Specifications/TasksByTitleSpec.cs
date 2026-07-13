using TaskManager_API.Core.Domain;

namespace Core.Application.Specifications;

public class TasksByTitleSpec : Specification<TaskItem>
{
    public TasksByTitleSpec(int userId, string searchTerm)
    {
        AddCriteria(t => t.UserId == userId && t.Title.Contains(searchTerm));
        ApplyOrderBy(t => t.Created);
    }
}