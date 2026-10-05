using TB.Domain.Entities;

namespace TB.Application.Abstractions;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) Generate(User user);
}
