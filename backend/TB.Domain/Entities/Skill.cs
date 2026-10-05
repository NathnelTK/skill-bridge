namespace TB.Domain.Entities;

public sealed class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<CandidateSkill> Candidates { get; } = new List<CandidateSkill>();
    public ICollection<JobSkill> Jobs { get; } = new List<JobSkill>();
}
