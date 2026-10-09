using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TB.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "applications",
                columns: new[] { "Id", "AppliedAtUtc", "CandidateId", "JobId", "Status" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000001"), "Received" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000002"), "Shortlisted" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "applications",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "applications",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));
        }
    }
}
