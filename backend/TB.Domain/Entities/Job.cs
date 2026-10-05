namespace TB.Domain.Entities;

public sealed class Job
{
    public Guid Id { get; set; }
    public Guid EmployerId { get; set; }
    public User Employer { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string JobType { get; set; } = "Full-time";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<JobSkill> JobSkills { get; set; } = [];
}
