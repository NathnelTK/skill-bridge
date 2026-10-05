using Microsoft.EntityFrameworkCore;
using TB.Application.Features.Applicants;
using TB.Application.Features.Applicants.Models;
using TB.Domain.Entities;
using TB.Infrastructure.Persistence;

namespace TB.Infrastructure.Services;

public sealed class CandidateProfileService : ICandidateProfileService
{
    private readonly AppDbContext _dbContext;

    public CandidateProfileService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CandidateProfileDto?> GetProfileAsync(
        Guid candidateId,
        CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Id == candidateId && user.Role == "Candidate",
                cancellationToken);

        if (candidate is null)
        {
            return null;
        }

        var skills = await _dbContext.CandidateSkills
            .AsNoTracking()
            .Where(candidateSkill => candidateSkill.CandidateId == candidateId)
            .OrderBy(candidateSkill => candidateSkill.Skill.Name)
            .Select(candidateSkill => new CandidateSkillDto
            {
                Id = candidateSkill.SkillId,
                Name = candidateSkill.Skill.Name
            })
            .ToListAsync(cancellationToken);

        return new CandidateProfileDto
        {
            Id = candidate.Id,
            Email = candidate.Email,
            FullName = candidate.FullName,
            Location = candidate.Location,
            Skills = skills
        };
    }

    public async Task<CandidateProfileDto?> UpdateProfileAsync(
        Guid candidateId,
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Id == candidateId && user.Role == "Candidate",
                cancellationToken);

        if (candidate is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ArgumentException("Full name is required.");
        }

        if (request.FullName.Trim().Length > 160)
        {
            throw new ArgumentException(
                "Full name cannot be longer than 160 characters.");
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

        candidate.FullName = request.FullName.Trim();

        candidate.Location = string.IsNullOrWhiteSpace(request.Location)
            ? null
            : request.Location.Trim();

        var existingSkills = await _dbContext.CandidateSkills
            .Where(candidateSkill => candidateSkill.CandidateId == candidateId)
            .ToListAsync(cancellationToken);

        _dbContext.CandidateSkills.RemoveRange(existingSkills);

        var newSkills = skillIds.Select(skillId => new CandidateSkill
        {
            CandidateId = candidateId,
            SkillId = skillId
        });

        await _dbContext.CandidateSkills.AddRangeAsync(
            newSkills,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetProfileAsync(
            candidateId,
            cancellationToken);
    }
}
