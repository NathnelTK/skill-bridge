using TB.Application.Abstractions;
using TB.Application.Common.Exceptions;
using TB.Application.Skills;
using TB.Domain.Entities;

namespace TB.Application.Candidates;

public sealed class CandidateProfileService(
    IUserRepository users,
    ISkillRepository skills) : ICandidateProfileService
{
    public async Task<CandidateProfileDto> GetCurrentAsync(Guid userId, CancellationToken ct)
    {
        var user = await users.GetByIdWithSkillsAsync(userId, ct)
            ?? throw new NotFoundException("The current user no longer exists.");

        return ToDto(user);
    }

    public async Task<CandidateProfileDto> UpdateCurrentAsync(
        Guid userId,
        UpdateCandidateProfileRequest request,
        CancellationToken ct)
    {
        var user = await users.GetByIdWithSkillsAsync(userId, ct)
            ?? throw new NotFoundException("The current user no longer exists.");

        var skillIds = request.SkillIds.Distinct().ToList();
        if (skillIds.Count > 0 && !await skills.AllExistAsync(skillIds, ct))
        {
            throw new ValidationException("One or more of the supplied skill ids do not exist.");
        }

        user.FullName = request.FullName.Trim();
        user.Location = string.IsNullOrWhiteSpace(request.Location) ? null : request.Location.Trim();

        await users.UpdateWithSkillsAsync(user, skillIds, ct);

        return await GetCurrentAsync(userId, ct);
    }

    private static CandidateProfileDto ToDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Location = user.Location,
        Skills = user.CandidateSkills
            .Select(candidateSkill => new SkillDto
            {
                Id = candidateSkill.SkillId,
                Name = candidateSkill.Skill.Name
            })
            .OrderBy(skill => skill.Name)
            .ToList()
    };
}
