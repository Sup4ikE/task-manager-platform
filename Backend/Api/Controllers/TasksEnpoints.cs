using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using Shared.Models;
using TaskManager_API.Data;

namespace TaskManager_API.Controllers;

public static class TasksEnpoints
{
    public static RouteGroupBuilder MapTasksEnpoints(this WebApplication app)
    {
        var group = app.MapGroup("tasks").RequireAuthorization();

        group.MapGet("/", async (Context db, HttpContext http) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Results.Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var tasks = await db.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Created = t.Created
                })
                .ToListAsync();

            return Results.Ok(tasks);
        });

        group.MapGet("/{id}", async (Context db, HttpContext http, int id) =>
        {
            var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var task = await db.Tasks
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => new TaskDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Created = t.Created
                })
                .FirstOrDefaultAsync();

            return task == null ? Results.NotFound() : Results.Ok(task);
        });


        //  "2025-08-14T00:00:00Z"
        group.MapPost("/", async (Context db, HttpContext http, TaskDTO newTaskDTO) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            if (newTaskDTO == null)
                return Results.BadRequest();

            var newTask = new TaskItem
            {
                Title = newTaskDTO.Title,
                Description = newTaskDTO.Description,
                Created = DateTime.UtcNow,
                IsCompleted = newTaskDTO.IsCompleted,
                UserId = userId 
            };

            db.Tasks.Add(newTask); 
            await db.SaveChangesAsync();

            return Results.Created($"/tasks/{newTask.Id}", newTask);
        });

        group.MapPut("/{id}", async (Context db, HttpContext http, int id, TaskDTO updateTaskDTO) =>
        {
            var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) return Results.NotFound();

            task.Title = updateTaskDTO.Title;
            task.Description = updateTaskDTO.Description;
            task.IsCompleted = updateTaskDTO.IsCompleted;
            task.Created = updateTaskDTO.Created;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (Context db, HttpContext http, int id) =>
        {
            var userId = int.Parse(http.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) return Results.NotFound();

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

            
        return group;
    }
}