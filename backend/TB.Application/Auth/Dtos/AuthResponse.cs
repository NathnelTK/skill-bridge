namespace TB.Application.Auth.DTOs;

public sealed record AuthResponse(
    string AccessToken,
    Guid UserId,
    string Email,
    string FullName,
    string Role
);