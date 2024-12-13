using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmaHauJenHoaVij.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b02936c-3b78-4f72-aa30-5517bcc68a36"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9207cd38-e1ea-4e6f-aa34-d6be04dc4697"));

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Picture = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("2d4df331-521e-4105-ac7a-5f2a63addfa3"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" },
                    { new Guid("7c8f300f-d7aa-4817-acca-8a02ae9f651f"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2d4df331-521e-4105-ac7a-5f2a63addfa3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7c8f300f-d7aa-4817-acca-8a02ae9f651f"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordNeedsReset", "UserType" },
                values: new object[,]
                {
                    { new Guid("3b02936c-3b78-4f72-aa30-5517bcc68a36"), "admin2@example.com", "Admin2", "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=", true, "Administrator" },
                    { new Guid("9207cd38-e1ea-4e6f-aa34-d6be04dc4697"), "admin@example.com", "Admin", "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=", true, "Administrator" }
                });
        }
    }
}
