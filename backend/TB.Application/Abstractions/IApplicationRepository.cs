using TB.Domain.Entities;
using ApplicationEntity = TB.Domain.Entities.Application;

namespace TB.Application.Abstractions;

public interface IApplicationRepository
{
    Task<ApplicationEntity?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<bool> ExistsAsync(Guid jobId, Guid candidateId, CancellationToken ct);

    Task<IReadOnlyList<ApplicationEntity>> ListForCandidateAsync(Guid candidateId, CancellationToken ct);

    Task<IReadOnlyList<ApplicationEntity>> ListForJobAsync(Guid jobId, CancellationToken ct);

    Task AddAsync(ApplicationEntity application, CancellationToken ct);

    Task UpdateAsync(ApplicationEntity application, CancellationToken ct);
}
