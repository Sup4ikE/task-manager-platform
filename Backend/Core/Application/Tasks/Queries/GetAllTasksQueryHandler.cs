using Core.Application.Specifications;
using MediatR;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Queries;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskItem>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TaskItem>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0) return new List<TaskItem>();

        return await _unitOfWork.Tasks.ListAsync(new AllUserTasksSpec(request.UserId));
    }
}