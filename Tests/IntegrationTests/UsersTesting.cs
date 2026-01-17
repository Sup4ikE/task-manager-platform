using Xunit; 
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

public class UsersTesting: IClassFixture<WebApplicationFactory<Program>> 
{
    private WebApplicationFactory<Program> _factory; 
    
    public UsersTesting() 
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => 
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TaskManagerContext>)); 
                if (descriptor != null) services.Remove(descriptor);
                
                services.AddDbContext<TaskManagerContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb1").UseInternalServiceProvider( new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider());
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
    public async Task GetUsers_ReturnsOk() 
    { 
        //Arrange 
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<TaskManagerContext>(); 
            await db.Database.EnsureDeletedAsync(); 
            await db.Database.EnsureCreatedAsync();
            
            db.Users.Add(new User
            {
                Username = "TestUser", 
                PasswordHash = "User",
                Role = "Admin"
            }); 
            
            await db.SaveChangesAsync();
        } 
        var client = _factory.CreateClient(); 
        
        // Act
        var response = await client.GetAsync("/users");
        var result = await response.Content.ReadFromJsonAsync<List<UserResponseDTO>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        //result.Should().HaveCount(1); 
        // result[0].Username.Should().Be("TestUser");
        // result[0].Role.Should().Be("Admin");
    } 
}
