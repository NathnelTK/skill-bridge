using DotNetEnv;
using TB.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var envFile = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));

if (File.Exists(envFile))
{
    Env.Load(envFile);
    builder.Configuration.AddEnvironmentVariables();
}

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapControllers();

app.Run();
