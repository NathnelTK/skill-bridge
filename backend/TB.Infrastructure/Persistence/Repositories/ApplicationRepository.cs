using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions;
using TB.Domain.Entities;
using ApplicationEntity = TB.Domain.Entities.Application;

namespace TB.Infrastructure.Persistence.Repositories;

public sealed class ApplicationRepository(AppDbContext context) : IApplicationRepository
{
    public Task<ApplicationEntity?> GetByIdAsync(Guid id, CancellationToken ct) =>
        context.Applications.FirstOrDefaultAsync(application => application.Id == id, ct);

    public Task<bool> ExistsAsync(Guid jobId, Guid candidateId, CancellationToken ct) =>
        context.Applications
            .AsNoTracking()
            .AnyAsync(
                application => application.JobId == jobId && application.CandidateId == candidateId,
                ct);

    public async Task<IReadOnlyList<ApplicationEntity>> ListForCandidateAsync(
        Guid candidateId,
        CancellationToken ct) =>
        await context.Applications
            .AsNoTracking()
            .Include(application => application.Job)
            .ThenInclude(job => job.Employer)
            .Include(application => application.Job)
            .ThenInclude(job => job.RequiredSkills)
            .ThenInclude(jobSkill => jobSkill.Skill)
            .Where(application => application.CandidateId == candidateId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ApplicationEntity>> ListForJobAsync(Guid jobId, CancellationToken ct) =>
        await context.Applications
            .AsNoTracking()
            .Include(application => application.Candidate)
            .ThenInclude(candidate => candidate.CandidateSkills)
            .ThenInclude(candidateSkill => candidateSkill.Skill)
            .Where(application => application.JobId == jobId)
            .ToListAsync(ct);

    public async Task AddAsync(ApplicationEntity application, CancellationToken ct)
    {
        context.Applications.Add(application);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ApplicationEntity application, CancellationToken ct)
    {
        context.Applications.Update(application);
        await context.SaveChangesAsync(ct);
    }
}
