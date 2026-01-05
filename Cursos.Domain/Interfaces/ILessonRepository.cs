using Cursos.Domain.Entities;

namespace Cursos.Domain.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<IEnumerable<Lesson>> GeyByCourseIdAsync(Guid courseId);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task<bool> OrderExistsAsync(Guid courseId, int order);
}