using Core.Application.Tasks.Commands;
using Core.Application.Tasks.Queries;
using Moq;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Domain;

namespace Tests.Unit;

public class HendlersTests
{
    [Fact]
    public async Task GetAllTasksQueryHandlerWith2tasks()
    {
        // Arrange
        var mockUow = new Mock<IUnitOfWork>();

        List<TaskItem> tasks = new List<TaskItem>()
        {
            new TaskItem { Id = 1, Title = "Test task1", UserId = 5 },
            new TaskItem { Id = 2, Title = "Test task2", UserId = 5 },
        };

        mockUow.Setup(u => u.Tasks.GetAllAsync(5))
            .ReturnsAsync(tasks);

        var handler = new GetAllTasksQueryHandler(mockUow.Object);
        var query = new GetAllTasksQuery(5);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

    }

    [Fact]
    public async Task UpdateTaskCommandHandlerMissingTask()
    {
        // Arrange
        var mockUow = new Mock<IUnitOfWork>();

        mockUow.Setup(u => u.Tasks.GetByIdAsync(5, 1))
            .ReturnsAsync((TaskItem?)null);

        var handler = new UpdateTaskCommandHandler(mockUow.Object);
        var command = new UpdateTaskCommand(5, 1, "New title", "New description", true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        mockUow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateTaskCommandHandlerSuccess()
    {
        // Arrange
        var mockUow = new Mock<IUnitOfWork>();

        var task = new TaskItem
        {
            Id = 1,
            UserId = 5,
            Title = "Old title",
            Description = "Old description",
            IsCompleted = false
        };

        mockUow.Setup(u => u.Tasks.GetByIdAsync(5, 1))
            .ReturnsAsync(task);

        var handler = new UpdateTaskCommandHandler(mockUow.Object);
        var command = new UpdateTaskCommand(5, 1, "New title", "New description", true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal("New title", task.Title);
        Assert.Equal("New description", task.Description);
        Assert.True(task.IsCompleted);
        mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskCommandHandlerCallsDeleteOnce()
    {
        // Arrange
        var mockUow = new Mock<IUnitOfWork>();

        mockUow.Setup(u => u.Tasks.DeleteByIdAsync(5, 1))
            .ReturnsAsync(true);

        var handler = new DeleteTaskCommandHandler(mockUow.Object);
        var command = new DeleteTaskCommand(5, 1);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mockUow.Verify(u => u.Tasks.DeleteByIdAsync(5, 1), Times.Once);
        mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}