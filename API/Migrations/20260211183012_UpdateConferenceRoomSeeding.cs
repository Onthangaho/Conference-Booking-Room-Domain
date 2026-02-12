using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConferenceRoomSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2026, 2, 13, 19, 30, 10, 313, DateTimeKind.Utc).AddTicks(945), new DateTime(2026, 2, 13, 18, 30, 10, 313, DateTimeKind.Utc).AddTicks(938) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2026, 2, 13, 6, 44, 35, 90, DateTimeKind.Utc).AddTicks(7246), new DateTime(2026, 2, 13, 5, 44, 35, 90, DateTimeKind.Utc).AddTicks(7239) });
        }
    }
}
