using TB.Application.Auth.DTOs;
using TB.Application.Common.Interfaces;
using TB.Domain.Entities;
using TB.Infrastructure.Persistence;
namespace TB.Application.Common.Interfaces;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public Task<AuthResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> RegisterAsync(string email, string password, string fullName, string role, string? companyName, string? location, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}