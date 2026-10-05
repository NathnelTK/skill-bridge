using TB.Application.Skills;

namespace TB.Application.Jobs;

public sealed class JobSummaryDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public int RequiredSkillCount { get; init; }

    public IReadOnlyList<SkillDto> RequiredSkills { get; init; } = [];
}
