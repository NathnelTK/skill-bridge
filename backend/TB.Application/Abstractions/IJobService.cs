using TB.Application.Jobs;

namespace TB.Application.Abstractions;

public interface IJobService
{
    Task<IReadOnlyList<JobSummaryDto>> BrowseAsync(
        IReadOnlyCollection<Guid> requiredSkillIds,
        CancellationToken ct);

    Task<JobDto> GetAsync(Guid jobId, CancellationToken ct);

    Task<IReadOnlyList<JobSummaryDto>> ListForCurrentEmployerAsync(Guid employerId, CancellationToken ct);

    Task<JobDto> CreateAsync(Guid employerId, CreateJobRequest request, CancellationToken ct);

    Task<JobDto> UpdateAsync(Guid employerId, Guid jobId, UpdateJobRequest request, CancellationToken ct);

    Task DeleteAsync(Guid employerId, Guid jobId, CancellationToken ct);

    Task<IReadOnlyList<ApplicantDto>> GetApplicantsAsync(Guid employerId, Guid jobId, CancellationToken ct);
}
