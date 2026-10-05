using System.ComponentModel.DataAnnotations;

namespace TB.Application.Auth.Register;

public class RegisterRequest
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    [Required, MinLength(6), StringLength(100)]
    public string Password { get; set; } = "";

    [Required]
    public string Role { get; set; } = "";

    [StringLength(100)]
    public string? CompanyName { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }
}

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}

public record AuthResponse(Guid UserId, string FullName, string Role, string Token, DateTime ExpiresAtUtc);