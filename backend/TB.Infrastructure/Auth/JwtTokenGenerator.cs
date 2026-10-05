using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TB.Application.Abstractions;
using TB.Domain.Entities;

namespace TB.Infrastructure.Auth;

public sealed class JwtTokenGenerator(JwtOptions options) : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options;

    public (string Token, DateTime ExpiresAtUtc) Generate(User user)
    {
        if (string.IsNullOrWhiteSpace(_options.Key))
        {
            throw new InvalidOperationException("Jwt:Key is not configured.");
        }

        var issuedAt = DateTime.UtcNow;
        var expiresAt = issuedAt.AddHours(_options.ExpiryHours);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = issuedAt,
            NotBefore = issuedAt,
            Expires = expiresAt,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);

        return (token, expiresAt);
    }
}
