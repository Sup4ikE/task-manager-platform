using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Models;
using TaskManager_API.Data;

namespace TaskManager_API.Controllers;

public static class UsersEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users").RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        group.MapGet("/", async (Context context) =>
        {
            var users = await context.Users.ToListAsync();
            var dtoList = users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            }).ToList();

            return Results.Ok(dtoList);
        });

        group.MapGet("/{id}", async (Context context, int id) =>
        {
            var user = await context.Users.FindAsync(id);

            if (user == null) return Results.NotFound();

            var dto = new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return Results.Ok(dto);
        });

        group.MapDelete("/{id}", async (Context context, int id) =>
        {
            var user = await context.Users.FindAsync(id);

            if (user == null) return Results.NotFound();

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            var dto = new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return Results.Ok(dto);
        });
        
        
        return group;
    }
}
