using TB.Domain.Entities;

namespace TB.Application.Abstractions;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Job?> GetByIdWithSkillsAsync(Guid id, CancellationToken ct);

    Task<IReadOnlyList<Job>> ListAsync(Guid? requiredSkillId, CancellationToken ct);

    Task<IReadOnlyList<Job>> ListForEmployerAsync(Guid employerId, CancellationToken ct);

    Task AddAsync(Job job, CancellationToken ct);

    Task UpdateAsync(Job job, IReadOnlyCollection<Guid> requiredSkillIds, CancellationToken ct);

    Task DeleteAsync(Job job, CancellationToken ct);
}
