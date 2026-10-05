namespace TB.Application.Features.Applicants.Models;

public sealed class UpdateCandidateProfileRequest
{
    public string FullName { get; init; } = string.Empty;

    public string? Location { get; init; }

    public List<Guid> SkillIds { get; init; } = [];
}