using Microsoft.EntityFrameworkCore;

namespace TB.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);
