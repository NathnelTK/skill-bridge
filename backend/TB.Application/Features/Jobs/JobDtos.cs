namespace TB.Application.Features.Jobs;

public sealed class JobDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string JobType { get; init; } = string.Empty;
    public string EmployerName { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
    public List<SkillDto> Skills { get; init; } = [];
}

public sealed class SkillDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class CreateJobRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string JobType { get; init; } = "Full-time";
    public List<Guid> SkillIds { get; init; } = [];
}