using MediatR;

namespace Core.Application.Tasks.Commands;

public record DeleteTaskCommand(int UserId, int TaskId) : IRequest<bool>;