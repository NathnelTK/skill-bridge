using TB.Application.Skills;

namespace TB.Application.Candidates;

public sealed class CandidateProfileDto
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Location { get; init; }

    public IReadOnlyList<SkillDto> Skills { get; init; } = [];
}
