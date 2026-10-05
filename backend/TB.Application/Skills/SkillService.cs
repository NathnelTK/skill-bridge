using TB.Application.Abstractions;
using TB.Domain.Entities;
using SkillEntity = TB.Domain.Entities.Skill;

namespace TB.Application.Skills;

public sealed class SkillService(ISkillRepository skills) : ISkillService
{
    public async Task<IReadOnlyList<SkillDto>> ListAsync(CancellationToken ct)
    {
        var all = await skills.ListAsync(ct);
        return all.Select(ToDto).ToList();
    }

    private static SkillDto ToDto(SkillEntity skill) => new()
    {
        Id = skill.Id,
        Name = skill.Name
    };
}
