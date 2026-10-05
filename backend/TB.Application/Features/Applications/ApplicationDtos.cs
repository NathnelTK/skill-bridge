namespace TB.Application.Features.Applications;

public sealed class ApplicationDto
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public Guid CandidateId { get; init; }

    public string JobTitle { get; init; } = string.Empty;

    public string EmployerName { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime AppliedAtUtc { get; init; }
}

public sealed class CreateApplicationRequest
{
    public Guid JobId { get; init; }
}

public sealed class CandidateApplicationDto
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public string JobTitle { get; init; } = string.Empty;

    public string EmployerName { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime AppliedAtUtc { get; init; }
}