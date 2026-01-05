using Cursos.Application.DTO.Lesson;
using Cursos.Application.Interfaces;
using Cursos.Domain.Entities;
using Cursos.Domain.Interfaces;

namespace Cursos.Application.Services;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;

    public LessonService(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }
    public async Task<Lesson> CreateAsync(CreateLessonDto dto)
    {
        var orderExists = await _lessonRepository.OrderExistsAsync(dto.CourseId, dto.Order);

        if (orderExists)
            throw new Exception("El orden ya existe dentro del curso");

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = dto.CourseId,
            Title = dto.Title,
            Order = dto.Order,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        await _lessonRepository.AddAsync(lesson);
        return lesson;
    }

    public async Task<Lesson> UpdateAsync(Guid id, UpdateLessonDto dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id)
            ?? throw new Exception("Leccion no encontrada");

        if (lesson.Order != dto.Order)
        {
            var orderExists = await _lessonRepository.OrderExistsAsync(
                lesson.CourseId, dto.Order);
            
            if (orderExists)
                throw new Exception("El orden ya existe dentro del curso");
            
            lesson.Order = dto.Order;
        }
        
        lesson.Title = dto.Title;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        await _lessonRepository.UpdateAsync(lesson);
        return lesson;
    }

    public async Task DeleteAsync(Guid id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id) 
            ?? throw new Exception("Leccion no encontrada");
        
        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        await _lessonRepository.UpdateAsync(lesson);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseAsync(Guid courseId)
    {
        return await _lessonRepository.GeyByCourseIdAsync(courseId);
    }

    public async Task MoveUpAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId)
            ?? throw new Exception("Leccion no encontrada");

        if (lesson.Order <= 1) return;
        
        var lessons = (await _lessonRepository
            .GeyByCourseIdAsync(lesson.CourseId))
            .Where(l => !l.IsDeleted)
            .ToList();
        
        var previous = lessons.FirstOrDefault(l => l.Order == lesson.Order - 1);

        if (previous == null) return;
        
        SwapOrder(lesson, previous);
        
        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.UpdateAsync(previous);
    }

    public async Task MoveDownAsync(Guid lessonId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId)
            ?? throw new Exception("Leccion no encontrada");
        
        var lessons = (await _lessonRepository
            .GeyByCourseIdAsync(lesson.CourseId))
            .Where(l => !l.IsDeleted)
            .ToList();
        
        var next = lessons.FirstOrDefault(l => l.Order == lesson.Order + 1);

        if (next == null) return;
        
        SwapOrder(lesson, next);
        
        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.UpdateAsync(next);
    }

    private static void SwapOrder(Lesson a, Lesson b)
    {
        var temp = a.Order;
        a.Order = b.Order;
        b.Order = temp;
        
        a.UpdatedAt = DateTime.UtcNow;
        b.UpdatedAt = DateTime.UtcNow;
    }
}