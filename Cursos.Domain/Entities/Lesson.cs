namespace Cursos.Domain.Entities;

public class Lesson
{
    public Guid Id { get; set; }
    
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    
    public string Title { get; set; } = String.Empty;
    public int Order { get; set; }
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}