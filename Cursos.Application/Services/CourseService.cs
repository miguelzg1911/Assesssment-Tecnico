using Cursos.Application.DTO.Course;
using Cursos.Application.Interfaces;
using Cursos.Domain.Entities;
using Cursos.Domain.Enums;
using Cursos.Domain.Interfaces;

namespace Cursos.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILessonRepository _lessonRepository;

    public CourseService(ICourseRepository courseRepository, ILessonRepository lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }
    public async Task<Course> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Status = CourseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        await _courseRepository.AddAsync(course);
        return course;
    }

    public async Task<Course> UpdateAsync(Guid id, UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(id) 
            ?? throw new Exception("Curso no encontrado");

        course.Title = dto.Title;
        course.Status = dto.Status;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);

        return course;
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id)
            ?? throw new Exception("Curso no encontrado");
        
        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;
        
        await _courseRepository.UpdateAsync(course);
    }

    public async Task PublishAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id)
            ?? throw new Exception("Curso no encontrado");

        var lessons = await _lessonRepository.GeyByCourseIdAsync(id);

        if (!lessons.Any(l => !l.IsDeleted))
            throw new Exception("No se puede publicar un curso sin leccion");
        
        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;
        
        await _courseRepository.UpdateAsync(course);
    }

    public async Task UnpublishAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id)
            ?? throw new Exception("Curso no encontrado");
        
        course.Status =  CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;
        
        await _courseRepository.UpdateAsync(course);
    }

    public Task<IEnumerable<Course>> SearchAsync(
        string? query, 
        CourseStatus? status, 
        int page, 
        int pageSize) => _courseRepository.SearchAsync(query, status, page, pageSize);
    
    public Task<int> CountAsync(string? query, CourseStatus? status)
        => _courseRepository.CountAsync(query, status);

    public async Task<CourseSummaryDto> GetSummaryAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id)
                     ?? throw new Exception("Curso no encontrado");

        return new CourseSummaryDto
        {
            Id = course.Id,
            Title = course.Title,
            Status = course.Status,
            TotalLessons = course.Lessons.Count(l => l.IsDeleted),
            UpdatedAt = course.UpdatedAt,
        };
    }
}