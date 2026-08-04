using Core.Application.Specifications;
using MediatR;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Commands;

public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, TaskItem>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<TaskItem> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0) throw new ArgumentException("Invalid user id", nameof(request.UserId));

        var taskItem = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            Created = DateTime.UtcNow
        };
        
        await _unitOfWork.Tasks.AddAsync(taskItem);
        await _unitOfWork.SaveChangesAsync();

        return taskItem;
    }
}