using TB.Application.Skills;
using TB.Domain.Enums;

namespace TB.Application.Applications;

public sealed class CandidateApplicationDto
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public string JobTitle { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string JobLocation { get; init; } = string.Empty;

    public ApplicationStatus Status { get; init; }

    public DateTime AppliedAtUtc { get; init; }

    public IReadOnlyList<SkillDto> RequiredSkills { get; init; } = [];
}
