using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class newmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("57a1fcfe-9ab9-4c53-9bb8-29b1e061f282"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0bf54f3-63bc-4633-a2ae-33dfbb442684"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "PasswordResetToken", "PasswordResetTokenExpiry", "UserType" },
                values: new object[,]
                {
                    { new Guid("1101e106-f86c-42d2-8d17-02cfd78598ea"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, null, null, "Administrator" },
                    { new Guid("793b94d3-9758-4aa3-870e-c1632e3540d8"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, null, null, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1101e106-f86c-42d2-8d17-02cfd78598ea"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("793b94d3-9758-4aa3-870e-c1632e3540d8"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "PasswordResetToken", "PasswordResetTokenExpiry", "UserType" },
                values: new object[,]
                {
                    { new Guid("57a1fcfe-9ab9-4c53-9bb8-29b1e061f282"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, null, null, "Administrator" },
                    { new Guid("b0bf54f3-63bc-4633-a2ae-33dfbb442684"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, null, null, "Administrator" }
                });
        }
    }
}
