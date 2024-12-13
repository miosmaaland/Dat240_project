using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class AddTipToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("03d3e1d3-1b93-4cb5-9b39-7e853b855a37"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f8f6b182-11c6-417d-8a0a-b179dd7de76e"));

            migrationBuilder.AddColumn<decimal>(
                name: "Tip",
                table: "Reimbursements",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tip",
                table: "Offers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tip",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("3aa3f4f6-2c4d-4fae-a4ef-0c0bff71ba85"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("ddad7ec1-247f-4047-b546-0cec3304e4a4"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3aa3f4f6-2c4d-4fae-a4ef-0c0bff71ba85"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ddad7ec1-247f-4047-b546-0cec3304e4a4"));

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Reimbursements");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Invoices");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("03d3e1d3-1b93-4cb5-9b39-7e853b855a37"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("f8f6b182-11c6-417d-8a0a-b179dd7de76e"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }
    }
}
