using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("025360da-4719-4ed1-922d-889ad7b16e53"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b557dd76-aa2e-4061-8925-49361cbf10f1"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "PasswordResetToken", "PasswordResetTokenExpiry", "Provider", "ProviderKey", "UserType" },
                values: new object[,]
                {
                    { new Guid("2580c18a-ba63-4391-bc94-a4cdcc1ca8f2"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, null, null, null, null, "Administrator" },
                    { new Guid("a83c7f84-7c55-458a-9be0-64eabb10add5"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, null, null, null, null, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2580c18a-ba63-4391-bc94-a4cdcc1ca8f2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a83c7f84-7c55-458a-9be0-64eabb10add5"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "PasswordResetToken", "PasswordResetTokenExpiry", "Provider", "ProviderKey", "UserType" },
                values: new object[,]
                {
                    { new Guid("025360da-4719-4ed1-922d-889ad7b16e53"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, null, null, null, null, "Administrator" },
                    { new Guid("b557dd76-aa2e-4061-8925-49361cbf10f1"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, null, null, null, null, "Administrator" }
                });
        }
    }
}
