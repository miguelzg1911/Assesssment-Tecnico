namespace Cursos.Application.DTO.Lesson;

public class CreateLessonDto
{
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
}