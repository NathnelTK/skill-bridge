using TB.Application.Skills;
using TB.Domain.Entities;

namespace TB.Application.Abstractions;

/// <summary>
/// Matches free-form CV text against the known skill vocabulary (normalising
/// synonyms and casing such as "C sharp" or "postgres" to the canonical skill).
/// </summary>
public interface ISkillExtractor
{
    IReadOnlyList<Skill> ExtractSkills(string cvText, IReadOnlyList<Skill> knownSkills);
}
