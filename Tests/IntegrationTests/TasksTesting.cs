using FluentAssertions; 
using Microsoft.AspNetCore.Mvc.Testing; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json; 
using TaskManager_API; 
using TaskManager_API.Data; 
using Microsoft.AspNetCore.Authentication; 
using TaskManager_API.Contracts.DTOs;
using TaskManager_API.Core.Domain;

public class TasksTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TasksTesting()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<TaskManagerContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<TaskManagerContext>(opt =>
                    opt.UseInMemoryDatabase("TestDb"));

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
            });
        });
    }

    [Fact]
    public async Task GetTasks_ReturnsOk()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            db.Tasks.Add(new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                Created = DateTime.UtcNow,
                IsCompleted = false,
                UserId = 1
            });

            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/tasks");
        var result = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Should().HaveCount(1);
        result[0].Title.Should().Be("Test Task");
    }
}
