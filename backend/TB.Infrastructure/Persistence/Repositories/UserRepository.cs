using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions;
using TB.Domain.Entities;
using TB.Infrastructure.Persistence;

namespace TB.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) =>
        context.Users.AsNoTracking().AnyAsync(user => user.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);
    }
}
