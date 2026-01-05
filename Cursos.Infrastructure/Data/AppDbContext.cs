using Cursos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cursos.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
}