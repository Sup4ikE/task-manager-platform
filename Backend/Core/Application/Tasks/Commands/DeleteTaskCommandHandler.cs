using MediatR;
using TaskManager_API.Core.Application.Interfaces;

namespace Core.Application.Tasks.Commands;

public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.TaskId <= 0) return false;

        var isDeleted = await _unitOfWork.Tasks.DeleteByIdAsync(request.UserId, request.TaskId);
        if (!isDeleted) return false;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}