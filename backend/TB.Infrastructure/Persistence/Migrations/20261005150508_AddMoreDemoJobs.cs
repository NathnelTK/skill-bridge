using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TB.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreDemoJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "jobs",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "EmployerId", "Location", "Title" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Improve deployment automation, observability, and reliability for our cloud services.", new Guid("10000000-0000-0000-0000-000000000001"), "Remote", "DevOps Engineer" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Build dependable data pipelines and PostgreSQL-backed services for our hiring platform.", new Guid("10000000-0000-0000-0000-000000000001"), "Addis Ababa (Hybrid)", "Data Engineer" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Design and maintain secure REST APIs that power our candidate and employer experiences.", new Guid("10000000-0000-0000-0000-000000000001"), "Remote", "API Developer" }
                });

            migrationBuilder.InsertData(
                table: "job_skills",
                columns: new[] { "JobId", "SkillId", "IsRequired" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000007"), true },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000008"), true },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000003"), true },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000007"), true },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000002"), true },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000003"), true },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000006"), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000008") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("20000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "job_skills",
                keyColumns: new[] { "JobId", "SkillId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("20000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "jobs",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "jobs",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "jobs",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"));
        }
    }
}
