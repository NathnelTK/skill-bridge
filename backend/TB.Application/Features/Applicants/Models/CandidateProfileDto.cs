namespace TB.Application.Features.Applicants.Models;

public sealed class CandidateProfileDto
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string? Location { get; init; }

    public List<CandidateSkillDto> Skills { get; init; } = [];
}

public sealed class CandidateSkillDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}