// using TB.Application.Features.Applicants.Models;

// namespace TB.Application.Features.Applicants;

// public interface ICandidateProfileService
// {
//     Task<CandidateProfileDto?> GetProfileAsync(
//         Guid candidateId,
//         CancellationToken cancellationToken = default);

//     Task<bool> UpdateProfileAsync(
//         Guid candidateId,
//         UpdateCandidateProfileRequest request,
//         CancellationToken cancellationToken = default);

//     Task<IReadOnlyList<CandidateSkillDto>> GetSkillsAsync(
//         Guid candidateId,
//         CancellationToken cancellationToken = default);

//     Task<bool> AddSkillAsync(
//         Guid candidateId,
//         Guid skillId,
//         CancellationToken cancellationToken = default);

//     Task<bool> RemoveSkillAsync(
//         Guid candidateId,
//         Guid skillId,
//         CancellationToken cancellationToken = default);
// }

using TB.Application.Features.Applicants.Models;

namespace TB.Application.Features.Applicants;

public interface ICandidateProfileService
{
    Task<CandidateProfileDto?> GetProfileAsync(
        Guid candidateId,
        CancellationToken cancellationToken = default);

    Task<CandidateProfileDto?> UpdateProfileAsync(
        Guid candidateId,
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken = default);
}