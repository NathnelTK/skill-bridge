using TB.Application.Skills;
using TB.Domain.Enums;

namespace TB.Application.Jobs;

public sealed class ApplicantDto
{
    public Guid ApplicationId { get; init; }

    public Guid CandidateId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Location { get; init; }

    public ApplicationStatus Status { get; init; }

    public DateTime AppliedAtUtc { get; init; }

    public int MatchPercent { get; init; }

    public int MatchedSkillCount { get; init; }

    public int RequiredSkillCount { get; init; }

    public IReadOnlyList<SkillDto> MatchedSkills { get; init; } = [];
}
