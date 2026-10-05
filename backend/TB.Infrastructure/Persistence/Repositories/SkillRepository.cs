using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions;
using TB.Domain.Entities;
using TB.Domain.Enums;

namespace TB.Infrastructure.Persistence.Repositories;

public sealed class SkillRepository(AppDbContext context) : ISkillRepository
{
    public async Task<IReadOnlyList<Skill>> ListAsync(CancellationToken ct) =>
        await context.Skills.AsNoTracking().OrderBy(skill => skill.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Skill>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken ct) =>
        await context.Skills
            .AsNoTracking()
            .Where(skill => ids.Contains(skill.Id))
            .OrderBy(skill => skill.Name)
            .ToListAsync(ct);

    public async Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct)
    {
        if (ids.Count == 0)
        {
            return true;
        }

        var found = await context.Skills
            .AsNoTracking()
            .Where(skill => ids.Contains(skill.Id))
            .Select(skill => skill.Id)
            .ToListAsync(ct);

        return found.Count == ids.Distinct().Count();
    }
}
