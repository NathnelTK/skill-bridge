using TB.Application.Abstractions;
using TB.Application.Common.Exceptions;
using TB.Application.Skills;
using TB.Domain.Entities;

namespace TB.Application.Candidates;

public sealed class CandidateProfileService(
    IUserRepository users,
    ISkillRepository skills,
    IPdfTextExtractor pdfTextExtractor,
    ISkillExtractor skillExtractor) : ICandidateProfileService
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

    public async Task<CvUploadResultDto> ImportCvAsync(
        Guid userId,
        Stream pdfStream,
        CancellationToken ct)
    {
        var user = await users.GetByIdWithSkillsAsync(userId, ct)
            ?? throw new NotFoundException("The current user no longer exists.");

        string cvText;
        try
        {
            cvText = pdfTextExtractor.ExtractText(pdfStream);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new ValidationException(
                "The uploaded file could not be read as a PDF. Please upload a valid PDF CV.");
        }

        if (string.IsNullOrWhiteSpace(cvText))
        {
            throw new ValidationException(
                "No text could be extracted from the PDF. Scanned or image-only CVs are not supported.");
        }

        var knownSkills = await skills.ListAsync(ct);
        var detected = skillExtractor.ExtractSkills(cvText, knownSkills);

        var existingSkillIds = user.CandidateSkills
            .Select(candidateSkill => candidateSkill.SkillId)
            .ToHashSet();

        var added = detected
            .Where(skill => !existingSkillIds.Contains(skill.Id))
            .ToList();

        var mergedSkillIds = existingSkillIds
            .Concat(added.Select(skill => skill.Id))
            .Distinct()
            .ToList();

        if (added.Count > 0)
        {
            await users.UpdateWithSkillsAsync(user, mergedSkillIds, ct);
        }

        return new CvUploadResultDto
        {
            AddedSkills = added.Select(skill => skill.Name).OrderBy(name => name).ToList(),
            AlreadyPresentSkills = detected
                .Where(skill => existingSkillIds.Contains(skill.Id))
                .Select(skill => skill.Name)
                .OrderBy(name => name)
                .ToList(),
            ExtractedCharacterCount = cvText.Length,
            Profile = await GetCurrentAsync(userId, ct)
        };
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
