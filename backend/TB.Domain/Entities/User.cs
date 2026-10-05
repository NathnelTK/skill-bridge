using TB.Domain.Enums;

namespace TB.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<CandidateSkill> CandidateSkills { get; } = new List<CandidateSkill>();
    public ICollection<Job> PostedJobs { get; } = new List<Job>();
    public ICollection<Application> Applications { get; } = new List<Application>();
}
