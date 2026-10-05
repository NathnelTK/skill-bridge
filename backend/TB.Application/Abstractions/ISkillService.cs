using TB.Application.Skills;

namespace TB.Application.Abstractions;

public interface ISkillService
{
    Task<IReadOnlyList<SkillDto>> ListAsync(CancellationToken ct);
}
