using Cursos.Domain.Entities;
using Cursos.Domain.Interfaces;
using Cursos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cursos.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Lesson>> GeyByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task AddAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> OrderExistsAsync(Guid courseId, int order)
    {
        return await _context.Lessons
            .AnyAsync(l => l.CourseId == courseId && l.Order == order);
    }
}