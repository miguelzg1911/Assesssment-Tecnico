using Cursos.Application.DTO.Lesson;
using Cursos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cursos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateLessonDto dto)
    {
        var lesson = await _lessonService.CreateAsync(dto);
        return Ok(lesson);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateLessonDto dto)
    {
        var lesson = await _lessonService.UpdateAsync(id, dto);
        return Ok(lesson);
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessonService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/up"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> MoveUp(Guid id)
    {
        await _lessonService.MoveUpAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/down"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> MoveDown(Guid id)
    {
        await _lessonService.MoveDownAsync(id);
        return NoContent();
    }

    // Lectura para todos los logueados
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseAsync(courseId);
        return Ok(lessons);
    }
}