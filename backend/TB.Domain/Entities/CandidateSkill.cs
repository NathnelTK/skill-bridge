namespace TB.Domain.Entities;

public sealed class CandidateSkill
{
    public Guid CandidateId { get; set; }
    public User Candidate { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
}
