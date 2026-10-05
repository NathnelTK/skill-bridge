using System.Text.Json.Serialization;
using DotNetEnv;
using TB.Api.Exceptions;
using TB.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var envFile = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", ".env"));

if (File.Exists(envFile))
{
    Env.Load(envFile);
    builder.Configuration.AddEnvironmentVariables();
}

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

const string FrontendCorsPolicy = "FrontendCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(FrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
