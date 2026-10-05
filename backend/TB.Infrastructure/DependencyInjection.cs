using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TB.Application.Features.Applicants;
using TB.Infrastructure.Persistence;

namespace TB.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "The database connection string 'DefaultConnection' is not configured. " +
                "Set ConnectionStrings__DefaultConnection in the environment.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICandidateProfileService,
            Services.CandidateProfileService>();

        return services;
    }
}
