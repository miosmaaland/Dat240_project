using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("27beee8a-2e0a-41bb-adc6-41caab28098a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("605e8acf-2c2b-4200-9c61-204d1dd02a86"));

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "PasswordResetToken", "PasswordResetTokenExpiry", "UserType" },
                values: new object[,]
                {
                    { new Guid("57a1fcfe-9ab9-4c53-9bb8-29b1e061f282"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, null, null, "Administrator" },
                    { new Guid("b0bf54f3-63bc-4633-a2ae-33dfbb442684"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, null, null, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("57a1fcfe-9ab9-4c53-9bb8-29b1e061f282"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0bf54f3-63bc-4633-a2ae-33dfbb442684"));

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("27beee8a-2e0a-41bb-adc6-41caab28098a"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("605e8acf-2c2b-4200-9c61-204d1dd02a86"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }
    }
}
