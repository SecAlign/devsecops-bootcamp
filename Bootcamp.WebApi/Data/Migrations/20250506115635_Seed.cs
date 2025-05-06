using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bootcamp.WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "api",
                table: "users",
                columns: new[] { "id", "email", "first_name", "last_name" },
                values: new object[,]
                {
                    { 1, "rami.yousef@example.com", "Rami", "Yousef" },
                    { 2, "lina.hassan@example.com", "Lina", "Hassan" },
                    { 3, "sami.odeh@example.com", "Sami", "Odeh" },
                    { 4, "nour.khalil@example.com", "Nour", "Khalil" },
                    { 5, "alaa.ahmad@example.com", "Alaa", "Ahmad" },
                    { 6, "faris.naser@example.com", "Faris", "Naser" },
                    { 7, "dana.salem@example.com", "Dana", "Salem" },
                    { 8, "khaled.mansour@example.com", "Khaled", "Mansour" },
                    { 9, "tariq.zayed@example.com", "Tariq", "Zayed" },
                    { 10, "reem.haddad@example.com", "Reem", "Haddad" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "api",
                table: "users",
                keyColumn: "id",
                keyValue: 10);
        }
    }
}
