using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager_API.Migrations
{
    /// <inheritdoc />
    public partial class Infff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Username" },
                values: new object[] { 1, "WayneWW", "", null, "", "Bruce" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Description", "IsCompleted", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "adasdadadadadasd", false, "Save_Gotham", 1 },
                    { 2, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "gggggggg", false, "Go", 1 }
                });
        }
    }
}
