using MediatR;
using TaskManager_API.Core.Domain;

namespace Core.Application.Tasks.Queries;

public record GetAllTasksQuery(int UserId) : IRequest<List<TaskItem>>;