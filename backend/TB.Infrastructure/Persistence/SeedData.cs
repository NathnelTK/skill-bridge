using TB.Domain.Entities;

namespace TB.Infrastructure.Persistence;

internal static class SeedData
{
    public static readonly Guid EmployerId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CandidateId = Guid.Parse("10000000-0000-0000-0000-000000000002");

    public static readonly Guid DotNetSkillId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid AspNetSkillId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid PostgresSkillId = Guid.Parse("20000000-0000-0000-0000-000000000003");
    public static readonly Guid AngularSkillId = Guid.Parse("20000000-0000-0000-0000-000000000004");
    public static readonly Guid TypeScriptSkillId = Guid.Parse("20000000-0000-0000-0000-000000000005");
    public static readonly Guid RestSkillId = Guid.Parse("20000000-0000-0000-0000-000000000006");
    public static readonly Guid DockerSkillId = Guid.Parse("20000000-0000-0000-0000-000000000007");
    public static readonly Guid GitSkillId = Guid.Parse("20000000-0000-0000-0000-000000000008");

    public static readonly Guid FullStackJobId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid BackendJobId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid FrontendJobId = Guid.Parse("30000000-0000-0000-0000-000000000003");

    public static readonly User[] Users =
    [
        new()
        {
            Id = EmployerId,
            Email = "employer@skillbridge.demo",
            PasswordHash = "!seed-only-account!",
            Role = "Employer",
            FullName = "Alex Morgan",
            CompanyName = "Northstar Labs",
            Location = "Addis Ababa",
            CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = CandidateId,
            Email = "candidate@skillbridge.demo",
            PasswordHash = "!seed-only-account!",
            Role = "Candidate",
            FullName = "Sam Taylor",
            Location = "Addis Ababa",
            CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    ];

    public static readonly Skill[] Skills =
    [
        new() { Id = DotNetSkillId, Name = "C#" },
        new() { Id = AspNetSkillId, Name = "ASP.NET Core" },
        new() { Id = PostgresSkillId, Name = "PostgreSQL" },
        new() { Id = AngularSkillId, Name = "Angular" },
        new() { Id = TypeScriptSkillId, Name = "TypeScript" },
        new() { Id = RestSkillId, Name = "REST APIs" },
        new() { Id = DockerSkillId, Name = "Docker" },
        new() { Id = GitSkillId, Name = "Git" }
    ];

    public static readonly CandidateSkill[] CandidateSkills =
    [
        new() { CandidateId = CandidateId, SkillId = DotNetSkillId },
        new() { CandidateId = CandidateId, SkillId = AspNetSkillId },
        new() { CandidateId = CandidateId, SkillId = PostgresSkillId },
        new() { CandidateId = CandidateId, SkillId = RestSkillId },
        new() { CandidateId = CandidateId, SkillId = GitSkillId }
    ];

    public static readonly Job[] Jobs =
    [
        new()
        {
            Id = FullStackJobId,
            EmployerId = EmployerId,
            Title = "Full-Stack .NET Developer",
            Description = "Build and improve hiring workflows across our .NET and Angular products.",
            Location = "Addis Ababa (Hybrid)",
            CreatedAtUtc = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = BackendJobId,
            EmployerId = EmployerId,
            Title = "Backend Engineer",
            Description = "Design reliable APIs and data services for a growing recruitment platform.",
            Location = "Remote",
            CreatedAtUtc = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = FrontendJobId,
            EmployerId = EmployerId,
            Title = "Angular Frontend Developer",
            Description = "Create clear, accessible experiences for candidates and employers.",
            Location = "Addis Ababa",
            CreatedAtUtc = new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc)
        }
    ];

    public static readonly JobSkill[] JobSkills =
    [
        new() { JobId = FullStackJobId, SkillId = DotNetSkillId },
        new() { JobId = FullStackJobId, SkillId = AspNetSkillId },
        new() { JobId = FullStackJobId, SkillId = AngularSkillId },
        new() { JobId = FullStackJobId, SkillId = PostgresSkillId },
        new() { JobId = BackendJobId, SkillId = DotNetSkillId },
        new() { JobId = BackendJobId, SkillId = AspNetSkillId },
        new() { JobId = BackendJobId, SkillId = RestSkillId },
        new() { JobId = BackendJobId, SkillId = DockerSkillId },
        new() { JobId = FrontendJobId, SkillId = AngularSkillId },
        new() { JobId = FrontendJobId, SkillId = TypeScriptSkillId },
        new() { JobId = FrontendJobId, SkillId = RestSkillId }
    ];
}
