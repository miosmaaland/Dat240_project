using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoicingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9fe345a7-54b5-48fc-8805-7e1560c68274"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dbcca975-02ed-460f-9a02-53c989dbcfe0"));

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("03d3e1d3-1b93-4cb5-9b39-7e853b855a37"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("f8f6b182-11c6-417d-8a0a-b179dd7de76e"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("03d3e1d3-1b93-4cb5-9b39-7e853b855a37"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f8f6b182-11c6-417d-8a0a-b179dd7de76e"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("9fe345a7-54b5-48fc-8805-7e1560c68274"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("dbcca975-02ed-460f-9a02-53c989dbcfe0"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }
    }
}
