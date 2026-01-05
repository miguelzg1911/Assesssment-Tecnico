using Cursos.Domain.Entities;
using Cursos.Domain.Enums;
using Cursos.Domain.Interfaces;
using Cursos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cursos.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Course>> SearchAsync(string? query, CourseStatus? status, int page, int pageSize)
    {
        IQueryable<Course> courses = _context.Courses;
        
        if (!string.IsNullOrEmpty(query))
            courses = courses.Where(c => c.Title.Contains(query));
        
        if (status.HasValue)
            courses = courses.Where(c => c.Status == status.Value);
        
        return await courses
            .OrderBy(c => c.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync(string? query, CourseStatus? status)
    {
        IQueryable<Course> courses = _context.Courses;

        if (!string.IsNullOrWhiteSpace(query))
            courses = courses.Where(c => c.Title.Contains(query));
        
        if (status.HasValue)
            courses = courses.Where(c => c.Status == status.Value);
        
        return await courses.CountAsync();
    }
}