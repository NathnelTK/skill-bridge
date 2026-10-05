using TB.Domain.Enums;

namespace TB.Domain.Entities;

public sealed class Application
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;
    public Guid CandidateId { get; set; }
    public User Candidate { get; set; } = null!;
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Received;
    public DateTime AppliedAtUtc { get; set; }

    public void SetStatus(ApplicationStatus status)
    {
        if (Status != ApplicationStatus.Received)
        {
            throw new InvalidOperationException(
                $"An application in {Status} status cannot transition to another status.");
        }

        if (status is not (ApplicationStatus.Shortlisted or ApplicationStatus.Rejected))
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Applications can only transition from Received to Shortlisted or Rejected.");
        }

        Status = status;
    }
}
