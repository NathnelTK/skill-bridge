using Microsoft.EntityFrameworkCore;
using TB.Application.Features.Jobs;
using TB.Domain.Entities;
using TB.Infrastructure.Persistence;

namespace TB.Infrastructure.Services;

public sealed class JobService : IJobService
{
    private readonly AppDbContext _dbContext;

    public JobService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<JobDto>> GetJobsAsync(
        string? skillFilter,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Jobs
            .AsNoTracking()
            .Include(job => job.Employer)
            .Include(job => job.JobSkills)
                .ThenInclude(jobSkill => jobSkill.Skill)
            .Where(job => job.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(skillFilter))
        {
            var filter = skillFilter.Trim();

            query = query.Where(job =>
                job.JobSkills.Any(jobSkill =>
                    EF.Functions.ILike(
                        jobSkill.Skill.Name,
                        $"%{filter}%")));
        }

        var jobs = await query
            .OrderByDescending(job => job.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return jobs
            .Select(MapToDto)
            .ToList();
    }

    public async Task<JobDto?> CreateJobAsync(
        Guid employerId,
        CreateJobRequest request,
        CancellationToken cancellationToken = default)
    {
        var employer = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Id == employerId &&
                        user.Role == "Employer",
                cancellationToken);

        if (employer is null)
        {
            throw new ArgumentException(
                "Employer not found or user is not an employer.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException(
                "Job title is required.");
        }

        if (request.Title.Trim().Length > 200)
        {
            throw new ArgumentException(
                "Job title cannot be longer than 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException(
                "Job description is required.");
        }

        if (string.IsNullOrWhiteSpace(request.JobType))
        {
            throw new ArgumentException(
                "Job type is required.");
        }

        if (request.JobType.Trim().Length > 50)
        {
            throw new ArgumentException(
                "Job type cannot be longer than 50 characters.");
        }

        if (request.Location?.Trim().Length > 160)
        {
            throw new ArgumentException(
                "Location cannot be longer than 160 characters.");
        }

        var skillIds = request.SkillIds
            .Distinct()
            .ToList();

        var validSkillIds = await _dbContext.Skills
            .Where(skill => skillIds.Contains(skill.Id))
            .Select(skill => skill.Id)
            .ToListAsync(cancellationToken);

        if (validSkillIds.Count != skillIds.Count)
        {
            throw new ArgumentException(
                "One or more selected skills do not exist.");
        }

        var job = new Job
        {
            Id = Guid.NewGuid(),
            EmployerId = employerId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Location = string.IsNullOrWhiteSpace(request.Location)
                ? null
                : request.Location.Trim(),
            JobType = request.JobType.Trim(),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (var skillId in skillIds)
        {
            job.JobSkills.Add(new JobSkill
            {
                JobId = job.Id,
                SkillId = skillId
            });
        }

        await _dbContext.Jobs.AddAsync(
            job,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return await _dbContext.Jobs
            .AsNoTracking()
            .Include(j => j.Employer)
            .Include(j => j.JobSkills)
                .ThenInclude(js => js.Skill)
            .Where(j => j.Id == job.Id)
            .Select(j => new JobDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                JobType = j.JobType,
                EmployerName = j.Employer.FullName,
                CreatedAtUtc = j.CreatedAtUtc,
                Skills = j.JobSkills
                    .OrderBy(js => js.Skill.Name)
                    .Select(js => new SkillDto
                    {
                        Id = js.SkillId,
                        Name = js.Skill.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static JobDto MapToDto(Job job)
    {
        return new JobDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            JobType = job.JobType,
            EmployerName = job.Employer.FullName,
            CreatedAtUtc = job.CreatedAtUtc,
            Skills = job.JobSkills
                .OrderBy(jobSkill => jobSkill.Skill.Name)
                .Select(jobSkill => new SkillDto
                {
                    Id = jobSkill.SkillId,
                    Name = jobSkill.Skill.Name
                })
                .ToList()
        };
    }
}
