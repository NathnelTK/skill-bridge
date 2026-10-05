using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TB.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Role = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Location = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "candidate_skills",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidate_skills", x => new { x.CandidateId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_candidate_skills_skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_candidate_skills_users_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jobs_users_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AppliedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applications_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applications_users_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "job_skills",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_skills", x => new { x.JobId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_job_skills_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_skills_skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "C#" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "ASP.NET Core" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "PostgreSQL" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Angular" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "TypeScript" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "REST APIs" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "Docker" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "Git" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CompanyName", "CreatedAtUtc", "Email", "FullName", "Location", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Northstar Labs", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "employer@skillbridge.demo", "Alex Morgan", "Addis Ababa", "!seed-only-account!", "Employer" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "candidate@skillbridge.demo", "Sam Taylor", "Addis Ababa", "!seed-only-account!", "Candidate" }
                });

            migrationBuilder.InsertData(
                table: "candidate_skills",
                columns: new[] { "CandidateId", "SkillId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000008") }
                });

            migrationBuilder.InsertData(
                table: "jobs",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "EmployerId", "Location", "Title" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Build and improve hiring workflows across our .NET and Angular products.", new Guid("10000000-0000-0000-0000-000000000001"), "Addis Ababa (Hybrid)", "Full-Stack .NET Developer" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Design reliable APIs and data services for a growing recruitment platform.", new Guid("10000000-0000-0000-0000-000000000001"), "Remote", "Backend Engineer" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Create clear, accessible experiences for candidates and employers.", new Guid("10000000-0000-0000-0000-000000000001"), "Addis Ababa", "Angular Frontend Developer" }
                });

            migrationBuilder.InsertData(
                table: "job_skills",
                columns: new[] { "JobId", "SkillId", "IsRequired" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), true },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000002"), true },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000003"), true },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000004"), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000001"), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002"), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000006"), true },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000007"), true },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000004"), true },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000005"), true },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000006"), true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_applications_CandidateId",
                table: "applications",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_JobId_CandidateId",
                table: "applications",
                columns: new[] { "JobId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_candidate_skills_SkillId",
                table: "candidate_skills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_job_skills_SkillId",
                table: "job_skills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_EmployerId",
                table: "jobs",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_skills_Name",
                table: "skills",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "candidate_skills");

            migrationBuilder.DropTable(
                name: "job_skills");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
