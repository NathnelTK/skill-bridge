using TB.Application.Abstractions;
using TB.Application.Applications;
using TB.Application.Common.Exceptions;
using TB.Application.Skills;
using TB.Domain.Entities;
using TB.Domain.Enums;
using ApplicationEntity = TB.Domain.Entities.Application;

namespace TB.Application.Applications;

public sealed class ApplicationService(
    IApplicationRepository applications,
    IJobRepository jobs) : IApplicationService
{
    public async Task<ApplicationDto> ApplyAsync(Guid candidateId, Guid jobId, CancellationToken ct)
    {
        var job = await jobs.GetByIdAsync(jobId, ct)
            ?? throw new NotFoundException("The requested job does not exist.");

        if (job.EmployerId == candidateId)
        {
            throw new ValidationException("You cannot apply to a job you posted.");
        }

        if (await applications.ExistsAsync(jobId, candidateId, ct))
        {
            throw new ValidationException("You have already applied to this job.");
        }

        var application = new ApplicationEntity
        {
            Id = Guid.NewGuid(),
            JobId = jobId,
            CandidateId = candidateId,
            AppliedAtUtc = DateTime.UtcNow
        };

        await applications.AddAsync(application, ct);

        return new ApplicationDto
        {
            Id = application.Id,
            JobId = application.JobId,
            CandidateId = application.CandidateId,
            Status = application.Status,
            AppliedAtUtc = application.AppliedAtUtc
        };
    }

    public async Task<IReadOnlyList<CandidateApplicationDto>> ListForCurrentCandidateAsync(
        Guid candidateId,
        CancellationToken ct)
    {
        var list = await applications.ListForCandidateAsync(candidateId, ct);

        return list
            .Select(application => new CandidateApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = application.Job.Title,
                CompanyName = application.Job.Employer.CompanyName ?? application.Job.Employer.FullName,
                JobLocation = application.Job.Location,
                Status = application.Status,
                AppliedAtUtc = application.AppliedAtUtc,
                RequiredSkills = application.Job.RequiredSkills
                    .Select(jobSkill => new SkillDto { Id = jobSkill.SkillId, Name = jobSkill.Skill.Name })
                    .OrderBy(skill => skill.Name)
                    .ToList()
            })
            .OrderByDescending(application => application.AppliedAtUtc)
            .ToList();
    }

    public async Task<ApplicationDto> UpdateStatusAsync(
        Guid employerId,
        Guid applicationId,
        UpdateApplicationStatusRequest request,
        CancellationToken ct)
    {
        var application = await applications.GetByIdAsync(applicationId, ct)
            ?? throw new NotFoundException("The requested application does not exist.");

        var job = await jobs.GetByIdAsync(application.JobId, ct)
            ?? throw new NotFoundException("The requested application does not exist.");

        if (job.EmployerId != employerId)
        {
            throw new ForbiddenException("You can only review applications for jobs you posted.");
        }

        if (request.Status is not (ApplicationStatus.Shortlisted or ApplicationStatus.Rejected))
        {
            throw new ValidationException("Applications can only be marked Shortlisted or Rejected.");
        }

        if (application.Status != ApplicationStatus.Received)
        {
            throw new ConflictException(
                $"This application is already {application.Status} and cannot be changed.");
        }

        application.SetStatus(request.Status);
        await applications.UpdateAsync(application, ct);

        return new ApplicationDto
        {
            Id = application.Id,
            JobId = application.JobId,
            CandidateId = application.CandidateId,
            Status = application.Status,
            AppliedAtUtc = application.AppliedAtUtc
        };
    }
}
