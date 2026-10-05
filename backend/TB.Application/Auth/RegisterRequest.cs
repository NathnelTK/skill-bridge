using System.ComponentModel.DataAnnotations;
using TB.Domain.Enums;

namespace TB.Application.Auth;

public sealed class RegisterRequest : IValidatableObject
{
    [Required]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(UserRole))]
    public UserRole Role { get; set; }

    [StringLength(160)]
    public string? CompanyName { get; set; }

    [StringLength(160)]
    public string? Location { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == UserRole.Employer && string.IsNullOrWhiteSpace(CompanyName))
        {
            yield return new ValidationResult(
                "CompanyName is required when registering as an employer.",
                [nameof(CompanyName)]);
        }
    }
}
