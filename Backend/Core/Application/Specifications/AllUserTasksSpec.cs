using TaskManager_API.Core.Domain;

namespace Core.Application.Specifications;

public class AllUserTasksSpec : Specification<TaskItem>
{
    public AllUserTasksSpec(int userId)
    {
        AddCriteria(t => t.UserId == userId);
    }
}