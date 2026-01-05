using Cursos.Domain.Enums;

namespace Cursos.Application.DTO.Course;

public class UpdateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public CourseStatus Status { get; set; }
}