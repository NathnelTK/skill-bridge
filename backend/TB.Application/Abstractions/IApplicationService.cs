using TB.Application.Applications;

namespace TB.Application.Abstractions;

public interface IApplicationService
{
    Task<ApplicationDto> ApplyAsync(Guid candidateId, Guid jobId, CancellationToken ct);

    Task<IReadOnlyList<CandidateApplicationDto>> ListForCurrentCandidateAsync(
        Guid candidateId,
        CancellationToken ct);

    Task<ApplicationDto> UpdateStatusAsync(
        Guid employerId,
        Guid applicationId,
        UpdateApplicationStatusRequest request,
        CancellationToken ct);
}
