using Cursos.Application.DTO.Auth;

namespace Cursos.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    Task<AuthResponseDto> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}