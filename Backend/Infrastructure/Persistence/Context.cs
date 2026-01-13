using Microsoft.EntityFrameworkCore;
using TaskManager_API.Core.Domain;

namespace TaskManager_API.Data;

public class Context(DbContextOptions<Context> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
}