namespace TB.Application.Features.Applications;

public interface IApplicationService
{
    Task<ApplicationDto?> CreateApplicationAsync(
        Guid candidateId,
        CreateApplicationRequest request,
        CancellationToken cancellationToken = default);

    Task<List<CandidateApplicationDto>> GetCandidateApplicationsAsync(
        Guid candidateId,
        CancellationToken cancellationToken = default);
}