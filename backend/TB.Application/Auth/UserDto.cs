using TB.Domain.Enums;

namespace TB.Application.Auth;

public sealed class UserDto
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public UserRole Role { get; init; }

    public string? CompanyName { get; init; }

    public string? Location { get; init; }
}
