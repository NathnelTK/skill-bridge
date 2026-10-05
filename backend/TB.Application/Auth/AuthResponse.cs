namespace TB.Application.Auth;

public sealed class AuthResponse
{
    public string Token { get; init; } = string.Empty;

    public DateTime ExpiresAtUtc { get; init; }

    public UserDto User { get; init; } = new();
}
