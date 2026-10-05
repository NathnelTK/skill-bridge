using TB.Application.Abstractions;
using TB.Application.Auth;
using TB.Application.Common.Exceptions;
using TB.Domain.Entities;
using TB.Domain.Enums;

namespace TB.Application.Auth;

public sealed class AuthService(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator tokenGenerator) : IAuthService
{
    private const string LegacySeedPasswordMarker = "!seed-only-account!";

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        if (!Enum.IsDefined(request.Role))
        {
            throw new ValidationException("The supplied role is not valid.");
        }

        var email = NormalizeEmail(request.Email);

        if (request.Role == UserRole.Employer && string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new ValidationException("CompanyName is required when registering as an employer.");
        }

        if (await users.EmailExistsAsync(email, ct))
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = request.Role,
            FullName = request.FullName.Trim(),
            CompanyName = string.IsNullOrWhiteSpace(request.CompanyName) ? null : request.CompanyName.Trim(),
            Location = string.IsNullOrWhiteSpace(request.Location) ? null : request.Location.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        await users.AddAsync(user, ct);

        return BuildResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = NormalizeEmail(request.Email);
        var user = await users.GetByEmailAsync(email, ct);

        if (user is null || user.PasswordHash == LegacySeedPasswordMarker)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        return BuildResponse(user);
    }

    public async Task<UserDto> GetCurrentAsync(Guid userId, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("The current user no longer exists.");

        return ToDto(user);
    }

    private AuthResponse BuildResponse(User user)
    {
        var (token, expiresAtUtc) = tokenGenerator.Generate(user);

        return new AuthResponse
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            User = ToDto(user)
        };
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        CompanyName = user.CompanyName,
        Location = user.Location
    };

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
