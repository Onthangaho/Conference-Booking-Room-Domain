using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class SeedConferenceRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "IsActive", "Location", "Name", "RoomType" },
                values: new object[,]
                {
                    { 1, 10, null, true, "Cape Town", "Room A", 0 },
                    { 2, 20, null, true, "Bloemfontein", "Room B", 2 }
                });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "Location", "Name", "RoomType" },
                values: new object[] { 3, 15, null, "Cape Town", "Room C", 1 });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "IsActive", "Location", "Name", "RoomType" },
                values: new object[] { 4, 25, null, true, "Bloemfontein", "Room D", 0 });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "Location", "Name", "RoomType" },
                values: new object[] { 5, 30, null, "Cape Town", "Room E", 2 });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "IsActive", "Location", "Name", "RoomType" },
                values: new object[,]
                {
                    { 6, 10, null, true, "Bloemfontein", "Room F", 1 },
                    { 7, 20, null, true, "Bloemfontein", "Room G", 0 },
                    { 8, 15, null, true, "Cape Town", "Room H", 2 },
                    { 9, 13, null, true, "Cape Town", "Room I", 1 },
                    { 10, 20, null, true, "Bloemfontein", "Room J", 0 },
                    { 11, 10, null, true, "Bloemfontein", "Room K", 2 },
                    { 12, 5, null, true, "Cape Town", "Room L", 1 },
                    { 13, 12, null, true, "Bloemfontein", "Room M", 0 }
                });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "Location", "Name", "RoomType" },
                values: new object[] { 14, 15, null, "Cape Town", "Room N", 2 });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Capacity", "DeletedAt", "IsActive", "Location", "Name", "RoomType" },
                values: new object[,]
                {
                    { 15, 12, null, true, "Cape Town", "Room O", 1 },
                    { 16, 30, null, true, "Cape Town", "Room P", 0 }
                });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2026, 2, 18, 19, 19, 14, 12, DateTimeKind.Utc).AddTicks(116), new DateTime(2026, 2, 18, 18, 19, 14, 12, DateTimeKind.Utc).AddTicks(110) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2026, 2, 17, 13, 42, 28, 832, DateTimeKind.Utc).AddTicks(8040), new DateTime(2026, 2, 17, 12, 42, 28, 832, DateTimeKind.Utc).AddTicks(8034) });
        }
    }
}
