using System.ComponentModel.DataAnnotations;
using TB.Domain.Enums;

namespace TB.Application.Applications;

public sealed class UpdateApplicationStatusRequest
{
    [Required]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; set; }
}
