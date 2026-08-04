using MediatR;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Queries;

public record GetByIdTaskQuery(int UserId, int TaskId) : IRequest<TaskItem?>;