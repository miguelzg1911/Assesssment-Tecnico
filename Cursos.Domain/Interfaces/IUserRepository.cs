using Cursos.Domain.Entities;

namespace Cursos.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}      