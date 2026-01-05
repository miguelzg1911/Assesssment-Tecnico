using Cursos.Application.DTO.Course;
using Cursos.Application.Interfaces;
using Cursos.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cursos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    
    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        var course = await _courseService.CreateAsync(dto);
        return Ok(course);
    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto)
    {
        var course = await _courseService.UpdateAsync(id, dto);
        return Ok(course);
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/publish"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _courseService.PublishAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/unpublish"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        await _courseService.UnpublishAsync(id);
        return NoContent();
    }

    // Endpoints de lectura
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string? query,
        CourseStatus? status,
        int page = 1,
        int pageSize = 10)
    {
        var items = await _courseService.SearchAsync(query, status, page, pageSize);
        var total = await _courseService.CountAsync(query, status);

        return Ok(new
        {
            items,
            total,
            page,
            pageSize
        });
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> Summary(Guid id)
    {
        var summary = await _courseService.GetSummaryAsync(id);
        return Ok(summary);
    }
}
