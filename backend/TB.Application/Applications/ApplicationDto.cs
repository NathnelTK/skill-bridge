using TB.Domain.Enums;

namespace TB.Application.Applications;

public sealed class ApplicationDto
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public Guid CandidateId { get; init; }

    public ApplicationStatus Status { get; init; }

    public DateTime AppliedAtUtc { get; init; }
}
