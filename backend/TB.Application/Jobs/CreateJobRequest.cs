using System.ComponentModel.DataAnnotations;

namespace TB.Application.Jobs;

public sealed class CreateJobRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Location { get; set; } = string.Empty;

    public IReadOnlyList<Guid> RequiredSkillIds { get; set; } = [];
}
