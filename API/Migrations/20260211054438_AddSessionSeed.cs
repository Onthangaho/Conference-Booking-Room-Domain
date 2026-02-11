using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Capacity", "End", "Start", "Title" },
                values: new object[] { 1, 10, new DateTime(2026, 2, 13, 6, 44, 35, 90, DateTimeKind.Utc).AddTicks(7246), new DateTime(2026, 2, 13, 5, 44, 35, 90, DateTimeKind.Utc).AddTicks(7239), "Daily Standup" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
