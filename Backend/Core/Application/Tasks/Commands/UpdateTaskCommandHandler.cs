using MediatR;
using TaskManager_API.Core.Application.Interfaces;

namespace Core.Application.Tasks.Commands;

public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.TaskId <= 0) return false;

        var task = await _unitOfWork.Tasks.GetByIdAsync(request.UserId, request.TaskId);
        if (task == null) return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.IsCompleted = request.IsCompleted;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
