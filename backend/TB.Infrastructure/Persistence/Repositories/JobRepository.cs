using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions;
using TB.Domain.Entities;

namespace TB.Infrastructure.Persistence.Repositories;

public sealed class JobRepository(AppDbContext context) : IJobRepository
{
    public Task<Job?> GetByIdAsync(Guid id, CancellationToken ct) =>
        context.Jobs.FirstOrDefaultAsync(job => job.Id == id, ct);

    public Task<Job?> GetByIdWithSkillsAsync(Guid id, CancellationToken ct) =>
        context.Jobs
            .AsNoTracking()
            .Include(job => job.Employer)
            .Include(job => job.RequiredSkills)
            .ThenInclude(jobSkill => jobSkill.Skill)
            .FirstOrDefaultAsync(job => job.Id == id, ct);

    public async Task<IReadOnlyList<Job>> ListAsync(
        IReadOnlyCollection<Guid> requiredSkillIds,
        CancellationToken ct)
    {
        var query = context.Jobs
            .AsNoTracking()
            .Include(job => job.Employer)
            .Include(job => job.RequiredSkills)
            .ThenInclude(jobSkill => jobSkill.Skill)
            .AsQueryable();

        if (requiredSkillIds.Count > 0)
        {
            query = query.Where(job =>
                job.RequiredSkills.Any(jobSkill => requiredSkillIds.Contains(jobSkill.SkillId)));
        }

        return await query
            .OrderByDescending(job => job.CreatedAtUtc)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Job>> ListForEmployerAsync(Guid employerId, CancellationToken ct) =>
        await context.Jobs
            .AsNoTracking()
            .Include(job => job.Employer)
            .Include(job => job.RequiredSkills)
            .ThenInclude(jobSkill => jobSkill.Skill)
            .Where(job => job.EmployerId == employerId)
            .OrderByDescending(job => job.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task AddAsync(Job job, CancellationToken ct)
    {
        context.Jobs.Add(job);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Job job, IReadOnlyCollection<Guid> requiredSkillIds, CancellationToken ct)
    {
        var desired = requiredSkillIds.ToHashSet();
        var existing = context.JobSkills
            .Where(jobSkill => jobSkill.JobId == job.Id)
            .ToList();

        foreach (var jobSkill in existing.Where(jobSkill => !desired.Contains(jobSkill.SkillId)))
        {
            context.JobSkills.Remove(jobSkill);
        }

        foreach (var skillId in desired
                     .Where(skillId => existing.All(jobSkill => jobSkill.SkillId != skillId)))
        {
            context.JobSkills.Add(new JobSkill { JobId = job.Id, SkillId = skillId, IsRequired = true });
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Job job, CancellationToken ct)
    {
        context.Jobs.Remove(job);
        await context.SaveChangesAsync(ct);
    }
}
