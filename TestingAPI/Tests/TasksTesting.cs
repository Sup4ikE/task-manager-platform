using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using TaskManager_API;
using TaskManager_API.Data;
using Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Shared.DTO;

public class TasksTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;

    public TasksTesting()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => 
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Context>)); 
                if (descriptor != null) services.Remove(descriptor);
                
                services.AddDbContext<Context>(options =>
                {
                    options.UseInMemoryDatabase("TestDb").UseInternalServiceProvider( new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider());
                }); 
                
                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { }); 
                services.PostConfigure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test"; 
                    options.DefaultChallengeScheme = "Test"; 
                });
            }); 
        });
    }

    [Fact]
    public async Task GetTasks_ReturnsOk()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Context>();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            db.Tasks.Add(new TaskItem
            {
                Id = 1,
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
        var response = await client.GetAsync("/tasks");
        var result = await response.Content.ReadFromJsonAsync<List<TaskDTO>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        //result.Should().HaveCount(1);
        // result![0].Title.Should().Be("Test Task");
        // result[0].Description.Should().Be("Test Description");
        // result[0].IsCompleted.Should().BeFalse();
    }
}