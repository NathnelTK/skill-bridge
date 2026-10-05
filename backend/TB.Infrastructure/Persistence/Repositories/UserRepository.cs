using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions;
using TB.Domain.Entities;
using TB.Infrastructure.Persistence;

namespace TB.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id, ct);

    public Task<User?> GetByIdWithSkillsAsync(Guid id, CancellationToken ct) =>
        context.Users
            .Include(user => user.CandidateSkills)
            .ThenInclude(candidateSkill => candidateSkill.Skill)
            .FirstOrDefaultAsync(user => user.Id == id, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) =>
        context.Users.AsNoTracking().AnyAsync(user => user.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateWithSkillsAsync(
        User user,
        IReadOnlyCollection<Guid> skillIds,
        CancellationToken ct)
    {
        var tracked = user;
        var desired = skillIds.ToHashSet();
        var existing = tracked.CandidateSkills.Select(candidateSkill => candidateSkill.SkillId).ToHashSet();

        foreach (var candidateSkill in tracked.CandidateSkills
                     .Where(candidateSkill => !desired.Contains(candidateSkill.SkillId))
                     .ToList())
        {
            tracked.CandidateSkills.Remove(candidateSkill);
        }

        foreach (var skillId in desired.Where(skillId => !existing.Contains(skillId)))
        {
            tracked.CandidateSkills.Add(new CandidateSkill
            {
                CandidateId = tracked.Id,
                SkillId = skillId
            });
        }

        await context.SaveChangesAsync(ct);
    }
}
