using Cursos.Domain.Entities;
using Cursos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cursos.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected AppDbContext()
    {
    }
}


public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}