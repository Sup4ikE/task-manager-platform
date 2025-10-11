using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace TaskManager_API.Data;

public class Context(DbContextOptions<Context> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // your db connection
    }
}