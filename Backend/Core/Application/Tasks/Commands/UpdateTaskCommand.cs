using MediatR;

namespace Core.Application.Tasks.Commands;

public record UpdateTaskCommand(int UserId, int TaskId, string Title, string Description, bool IsCompleted)
    : IRequest<bool>;
