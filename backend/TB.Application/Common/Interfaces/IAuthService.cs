using TB.Application.Auth;
using TB.Domain.Entities;
namespace TB.Application.Common.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(
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