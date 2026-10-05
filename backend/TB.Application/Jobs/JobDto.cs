using TB.Application.Skills;

namespace TB.Application.Jobs;

public sealed class JobDto
{
    public Guid Id { get; init; }

    public Guid EmployerId { get; init; }

    public string EmployerName { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public IReadOnlyList<SkillDto> RequiredSkills { get; init; } = [];
}
