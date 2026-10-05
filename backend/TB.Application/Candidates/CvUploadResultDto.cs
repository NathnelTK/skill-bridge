using TB.Application.Skills;

namespace TB.Application.Candidates;

public sealed class CvUploadResultDto
{
    /// <summary>Skill names that were detected in the CV and added to the profile.</summary>
    public IReadOnlyList<string> AddedSkills { get; init; } = [];

    /// <summary>Skill names that were detected in the CV but were already on the profile.</summary>
    public IReadOnlyList<string> AlreadyPresentSkills { get; init; } = [];

    /// <summary>Number of characters of text extracted from the PDF (0 for an image-only PDF).</summary>
    public int ExtractedCharacterCount { get; init; }

    public CandidateProfileDto Profile { get; init; } = new();
}
