using Cursos.Application.DTO.Auth;
using Cursos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cursos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result); // Devuelve AccessToken y RefreshToken
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result); // Devuelve AccessToken y RefreshToken
    }

    // REFRESH TOKEN
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshAsync(dto.RefreshToken);
        return Ok(result); // Devuelve nuevo AccessToken y RefreshToken
    }

    // LOGOUT
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
    {
        await _authService.LogoutAsync(dto.RefreshToken);
        return NoContent();
    }
}