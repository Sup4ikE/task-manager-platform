using MediatR;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Commands;

public record CreateTaskCommand(string Title, string Description, int UserId) 
    : IRequest<TaskItem>;