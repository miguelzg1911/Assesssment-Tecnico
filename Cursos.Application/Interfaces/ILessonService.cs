using Cursos.Application.DTO.Lesson;
using Cursos.Domain.Entities;

namespace Cursos.Application.Interfaces;

public interface ILessonService
{
    Task<Lesson> CreateAsync(CreateLessonDto dto);
    Task<Lesson> UpdateAsync(Guid id, UpdateLessonDto dto);
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<Lesson>> GetByCourseAsync(Guid courseId);
    
    Task MoveUpAsync(Guid lessonId);
    Task MoveDownAsync(Guid lessonId);
}