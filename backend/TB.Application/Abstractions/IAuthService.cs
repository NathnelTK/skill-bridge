using TB.Application.Auth;

namespace TB.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct);

    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);

    Task<UserDto> GetCurrentAsync(Guid userId, CancellationToken ct);
}
