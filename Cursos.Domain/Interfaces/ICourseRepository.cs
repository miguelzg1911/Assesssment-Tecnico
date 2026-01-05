using Cursos.Domain.Entities;
using Cursos.Domain.Enums;

namespace Cursos.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);

    Task<IEnumerable<Course>> SearchAsync(
        string? query,
        CourseStatus? status,
        int page,
        int pageSize
    );

    Task<int> CountAsync(
        string? query,
        CourseStatus? status
    );
}