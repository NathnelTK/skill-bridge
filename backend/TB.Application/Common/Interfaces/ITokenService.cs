using TB.Domain.Entities;

namespace TB.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}