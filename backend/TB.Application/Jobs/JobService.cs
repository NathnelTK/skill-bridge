using TB.Application.Abstractions;
using TB.Application.Common.Exceptions;
using TB.Application.Skills;
using TB.Domain.Entities;
using ApplicationEntity = TB.Domain.Entities.Application;

namespace TB.Application.Jobs;

public sealed class JobService(
    IJobRepository jobs,
    ISkillRepository skills,
    IApplicationRepository applications) : IJobService
{
    public async Task<IReadOnlyList<JobSummaryDto>> BrowseAsync(Guid? requiredSkillId, CancellationToken ct)
    {
        var list = await jobs.ListAsync(requiredSkillId, ct);
        return list.Select(ToSummary).ToList();
    }

    public async Task<JobDto> GetAsync(Guid jobId, CancellationToken ct)
    {
        var job = await jobs.GetByIdWithSkillsAsync(jobId, ct)
            ?? throw new NotFoundException("The requested job does not exist.");

        return ToDto(job);
    }

    public async Task<IReadOnlyList<JobSummaryDto>> ListForCurrentEmployerAsync(
        Guid employerId,
        CancellationToken ct)
    {
        var list = await jobs.ListForEmployerAsync(employerId, ct);
        return list.Select(ToSummary).ToList();
    }

    public async Task<JobDto> CreateAsync(Guid employerId, CreateJobRequest request, CancellationToken ct)
    {
        var skillIds = request.RequiredSkillIds.Distinct().ToList();
        await EnsureSkillsExistAsync(skillIds, ct);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            EmployerId = employerId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Location = request.Location.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (var skillId in skillIds)
        {
            job.RequiredSkills.Add(new JobSkill { JobId = job.Id, SkillId = skillId, IsRequired = true });
        }

        await jobs.AddAsync(job, ct);

        return await GetAsync(job.Id, ct);
    }

    public async Task<JobDto> UpdateAsync(
        Guid employerId,
        Guid jobId,
        UpdateJobRequest request,
        CancellationToken ct)
    {
        var job = await jobs.GetByIdAsync(jobId, ct)
            ?? throw new NotFoundException("The requested job does not exist.");

        EnsureOwnership(job, employerId);

        var skillIds = request.RequiredSkillIds.Distinct().ToList();
        await EnsureSkillsExistAsync(skillIds, ct);

        job.Title = request.Title.Trim();
        job.Description = request.Description.Trim();
        job.Location = request.Location.Trim();

        await jobs.UpdateAsync(job, skillIds, ct);

        return await GetAsync(job.Id, ct);
    }

    public async Task DeleteAsync(Guid employerId, Guid jobId, CancellationToken ct)
    {
        var job = await jobs.GetByIdAsync(jobId, ct)
            ?? throw new NotFoundException("The requested job does not exist.");

        EnsureOwnership(job, employerId);

        await jobs.DeleteAsync(job, ct);
    }

    public async Task<IReadOnlyList<ApplicantDto>> GetApplicantsAsync(
        Guid employerId,
        Guid jobId,
        CancellationToken ct)
    {
        var job = await jobs.GetByIdWithSkillsAsync(jobId, ct)
            ?? throw new NotFoundException("The requested job does not exist.");

        EnsureOwnership(job, employerId);

        var jobApplications = await applications.ListForJobAsync(jobId, ct);
        var requiredSkillIds = job.RequiredSkills.Select(jobSkill => jobSkill.SkillId).ToHashSet();

        return jobApplications
            .Select(application => BuildApplicant(application, requiredSkillIds))
            .OrderByDescending(applicant => applicant.MatchPercent)
            .ThenBy(applicant => applicant.AppliedAtUtc)
            .ToList();
    }

    private static ApplicantDto BuildApplicant(ApplicationEntity application, HashSet<Guid> requiredSkillIds)
    {
        var candidateSkills = application.Candidate.CandidateSkills
            .Where(candidateSkill => requiredSkillIds.Contains(candidateSkill.SkillId))
            .Select(candidateSkill => new SkillDto
            {
                Id = candidateSkill.SkillId,
                Name = candidateSkill.Skill.Name
            })
            .OrderBy(skill => skill.Name)
            .ToList();

        var matched = candidateSkills.Count;
        var required = requiredSkillIds.Count;
        var matchPercent = required == 0
            ? 100
            : (int)Math.Round(matched * 100d / required, MidpointRounding.AwayFromZero);

        return new ApplicantDto
        {
            ApplicationId = application.Id,
            CandidateId = application.CandidateId,
            FullName = application.Candidate.FullName,
            Email = application.Candidate.Email,
            Location = application.Candidate.Location,
            Status = application.Status,
            AppliedAtUtc = application.AppliedAtUtc,
            MatchPercent = matchPercent,
            MatchedSkillCount = matched,
            RequiredSkillCount = required,
            MatchedSkills = candidateSkills
        };
    }

    private async Task EnsureSkillsExistAsync(IReadOnlyCollection<Guid> skillIds, CancellationToken ct)
    {
        if (skillIds.Count > 0 && !await skills.AllExistAsync(skillIds, ct))
        {
            throw new ValidationException("One or more of the supplied skill ids do not exist.");
        }
    }

    private static void EnsureOwnership(Job job, Guid employerId)
    {
        if (job.EmployerId != employerId)
        {
            throw new ForbiddenException("You can only manage jobs that you posted.");
        }
    }

    private static JobSummaryDto ToSummary(Job job) => new()
    {
        Id = job.Id,
        Title = job.Title,
        Location = job.Location,
        CompanyName = job.Employer.CompanyName ?? job.Employer.FullName,
        CreatedAtUtc = job.CreatedAtUtc,
        RequiredSkillCount = job.RequiredSkills.Count,
        RequiredSkills = job.RequiredSkills
            .Select(jobSkill => new SkillDto { Id = jobSkill.SkillId, Name = jobSkill.Skill.Name })
            .OrderBy(skill => skill.Name)
            .ToList()
    };

    private static JobDto ToDto(Job job) => new()
    {
        Id = job.Id,
        EmployerId = job.EmployerId,
        EmployerName = job.Employer.FullName,
        CompanyName = job.Employer.CompanyName ?? job.Employer.FullName,
        Title = job.Title,
        Description = job.Description,
        Location = job.Location,
        CreatedAtUtc = job.CreatedAtUtc,
        RequiredSkills = job.RequiredSkills
            .Select(jobSkill => new SkillDto { Id = jobSkill.SkillId, Name = jobSkill.Skill.Name })
            .OrderBy(skill => skill.Name)
            .ToList()
    };
}
