using TB.Application.Auth.DTOs;

namespace TB.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(
        string email,
        string password,
        string fullName,
        string role,
        string? companyName,
        string? location,
        CancellationToken cancellationToken);

    Task<AuthResponse> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken);
}