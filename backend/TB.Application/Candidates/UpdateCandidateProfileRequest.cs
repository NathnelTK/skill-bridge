using System.ComponentModel.DataAnnotations;

namespace TB.Application.Candidates;

public sealed class UpdateCandidateProfileRequest
{
    [Required]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(160)]
    public string? Location { get; set; }

    public IReadOnlyList<Guid> SkillIds { get; set; } = [];
}
