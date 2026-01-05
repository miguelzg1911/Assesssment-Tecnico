using Cursos.Domain.Enums;

namespace Cursos.Application.DTO.Course;

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public CourseStatus Status { get; set; }
    public int TotalLessons { get; set; }
    public DateTime UpdatedAt { get; set; }
}