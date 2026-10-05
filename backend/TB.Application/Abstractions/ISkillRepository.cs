using TB.Domain.Entities;

namespace TB.Application.Abstractions;

public interface ISkillRepository
{
    Task<IReadOnlyList<Skill>> ListAsync(CancellationToken ct);

    Task<IReadOnlyList<Skill>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct);

    Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct);
}
