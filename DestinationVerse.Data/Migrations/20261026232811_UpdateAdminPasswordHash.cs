using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinationVerse.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-static-id-001",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBLsHjJ9XlPshCkqU7Lz9mK+YqF5uGv1YgZ2tN1vK6fXp3zGv9mK+YqF5uGv1YgZ2tN1==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-static-id-001",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGqP/Xb1vLfZ8J7gKq0uO4BvL6A==");
        }
    }
}
