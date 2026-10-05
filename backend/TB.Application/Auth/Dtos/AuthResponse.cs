namespace TB.Application.Auth.Dtos;


public sealed record AuthResponse(
    string AccessToken,
    Guid UserId,
    string Email,
    string FullName,
    string Role
);