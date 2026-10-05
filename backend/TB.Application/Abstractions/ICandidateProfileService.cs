using TB.Application.Candidates;

namespace TB.Application.Abstractions;

public interface ICandidateProfileService
{
    Task<CandidateProfileDto> GetCurrentAsync(Guid userId, CancellationToken ct);

    Task<CandidateProfileDto> UpdateCurrentAsync(
        Guid userId,
        UpdateCandidateProfileRequest request,
        CancellationToken ct);
}
