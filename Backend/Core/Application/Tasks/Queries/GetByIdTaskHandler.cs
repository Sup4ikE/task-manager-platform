using Core.Application.Specifications;
using MediatR;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Queries;

public class GetByIdTaskHandler: IRequestHandler<GetByIdTaskQuery, TaskItem>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdTaskHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskItem?> Handle(GetByIdTaskQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.TaskId <= 0) return null;

        var taskItem = await _unitOfWork.Tasks.GetByIdAsync(request.UserId, request.TaskId);
        
        return taskItem;
    }
}