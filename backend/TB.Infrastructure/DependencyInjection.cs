using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TB.Application.Abstractions;
using TB.Application.Applications;
using TB.Application.Auth;
using TB.Application.Candidates;
using TB.Application.Jobs;
using TB.Application.Skills;
using TB.Infrastructure.Auth;
using TB.Infrastructure.Cv;
using TB.Infrastructure.Persistence;
using TB.Infrastructure.Persistence.Repositories;

namespace TB.Infrastructure;

public static class DependencyInjection
{
    private const int MinimumSigningKeyLength = 32;

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

        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();
        ValidateSigningKey(jwtOptions.Key);

        services.AddSingleton(jwtOptions);
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPdfTextExtractor, PdfTextExtractor>();
        services.AddSingleton<ISkillExtractor, SkillNameExtractor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ICandidateProfileService, CandidateProfileService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IApplicationService, ApplicationService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key!)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static void ValidateSigningKey(string? key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Length < MinimumSigningKeyLength)
        {
            throw new InvalidOperationException(
                $"Jwt:Key is not configured or is shorter than {MinimumSigningKeyLength} characters. " +
                "Set Jwt__Key in the environment with a cryptographically random value.");
        }
    }
}
