namespace TB.Domain.Entities;

public sealed class Application
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;
    public Guid CandidateId { get; set; }
    public User Candidate { get; set; } = null!;
    public string Status { get; set; } = "Received";
    public DateTime AppliedAtUtc { get; set; }
}
