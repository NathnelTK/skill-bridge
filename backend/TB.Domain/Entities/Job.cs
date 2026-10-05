namespace TB.Domain.Entities;

public sealed class Job
{
    public Guid Id { get; set; }
    public Guid EmployerId { get; set; }
    public User Employer { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<JobSkill> RequiredSkills { get; } = new List<JobSkill>();
    public ICollection<Application> Applications { get; } = new List<Application>();
}
