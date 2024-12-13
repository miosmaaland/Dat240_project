using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToReimbursement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3aa3f4f6-2c4d-4fae-a4ef-0c0bff71ba85"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ddad7ec1-247f-4047-b546-0cec3304e4a4"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reimbursements",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("27beee8a-2e0a-41bb-adc6-41caab28098a"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("605e8acf-2c2b-4200-9c61-204d1dd02a86"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("27beee8a-2e0a-41bb-adc6-41caab28098a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("605e8acf-2c2b-4200-9c61-204d1dd02a86"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reimbursements");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("3aa3f4f6-2c4d-4fae-a4ef-0c0bff71ba85"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("ddad7ec1-247f-4047-b546-0cec3304e4a4"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }
    }
}
