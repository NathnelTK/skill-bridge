using TB.Domain.Entities;

namespace TB.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);

    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<User?> GetByIdWithSkillsAsync(Guid id, CancellationToken ct);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);

    Task UpdateWithSkillsAsync(User user, IReadOnlyCollection<Guid> skillIds, CancellationToken ct);
}
