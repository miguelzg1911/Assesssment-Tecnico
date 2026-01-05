using Cursos.Application.DTO.Course;
using Cursos.Domain.Entities;
using Cursos.Domain.Enums;

namespace Cursos.Application.Interfaces;

public interface ICourseService
{
    Task<Course> CreateAsync(CreateCourseDto dto);
    Task<Course> UpdateAsync(Guid id, UpdateCourseDto dto);
    Task DeleteAsync(Guid id);

    Task PublishAsync(Guid id);
    Task UnpublishAsync(Guid id);
    
    Task<IEnumerable<Course>> SearchAsync(
        string? query,
        CourseStatus? status,
        int page,
        int pageSize
        );
    
    Task<int> CountAsync(string? query, CourseStatus? status);
    
    Task<CourseSummaryDto> GetSummaryAsync(Guid id);
}