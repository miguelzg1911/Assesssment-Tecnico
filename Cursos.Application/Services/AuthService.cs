using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cursos.Application.DTO.Auth;
using Cursos.Application.Interfaces;
using Cursos.Domain.Entities;
using Cursos.Domain.Enums;
using Cursos.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace Cursos.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    // REGISTER
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("El correo ya está registrado");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Student
        };

        await _userRepository.AddAsync(user);

        // Generar tokens al registrar
        var tokens = await GenerateTokensAsync(user);

        return tokens;
    }

    // LOGIN
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Usuario o contraseña incorrectos");

        var tokens = await GenerateTokensAsync(user);
        return tokens;
    }

    // REFRESH TOKEN
    public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null)
            throw new Exception("Refresh token inválido");

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            user.RefreshToken = null;
            await _userRepository.UpdateAsync(user);
            throw new Exception("Refresh token expirado");
        }

        // Generar nuevos tokens (rotación)
        var tokens = await GenerateTokensAsync(user);
        return tokens;
    }

    // LOGOUT
    public async Task LogoutAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = default;
        await _userRepository.UpdateAsync(user);
    }

    // GENERAR ACCESS + REFRESH
    private async Task<AuthResponseDto> GenerateTokensAsync(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

        // Generar Refresh Token
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshToken
        };
    }

    // GENERAR REFRESH TOKEN RANDOM
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
