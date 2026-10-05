using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TB.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuthSeedPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$12$RbeGHvbfZA0bETn2OUbutue9R3Wt.WjtCmroac.e4Lajmrh7RRTX6");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "$2a$12$RbeGHvbfZA0bETn2OUbutue9R3Wt.WjtCmroac.e4Lajmrh7RRTX6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "!seed-only-account!");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "!seed-only-account!");
        }
    }
}
