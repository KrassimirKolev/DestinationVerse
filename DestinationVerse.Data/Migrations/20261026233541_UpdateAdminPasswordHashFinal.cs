using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinationVerse.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHashFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-static-id-001",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHUmeKUxXQn/Vmi9ondm5EZ9Cd3JIw3XWBvpSV2rmR1BZxg6v+abCHR21j8rBN9rsg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-static-id-001",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBLsHjJ9XlPshCkqU7Lz9mK+YqF5uGv1YgZ2tN1vK6fXp3zGv9mK+YqF5uGv1YgZ2tN1==");
        }
    }
}
