using Cursos.Domain.Enums;

namespace Cursos.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    
    public CourseStatus Status { get; set; }
}