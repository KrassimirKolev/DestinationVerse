using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DestinationVerse.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDestinationsAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-static-id-001", 0, "STATIC-CONCURRENCY-STAMP-001", "admin@destinationverse.com", true, false, null, "ADMIN@DESTINATIONVERSE.COM", "ADMIN@DESTINATIONVERSE.COM", "AQAAAAIAAYagAAAAEGqP/Xb1vLfZ8J7gKq0uO4BvL6A==", null, false, "STATIC-SECURITY-STAMP-001", false, "admin@destinationverse.com" });

            migrationBuilder.InsertData(
                table: "Destinations",
                columns: new[] { "Id", "CoverImageUrl", "Description", "IsDeleted", "PublishedOn", "PublisherId", "RegionId", "Title" },
                values: new object[,]
                {
                    { 1, "https://unsplash.com", "Група езера с ледников произход, разположени в Северозападна Рила. Най-посещаваната езерна група в България, включваща Сълзата, Окото, Бъбрека, Близнака, Трилистника, Рибното и Долното езеро.", false, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin-user-static-id-001", 1, "Седемте рилски езера" },
                    { 2, "https://unsplash.com", "Най-високият връх на Пирин планина със своите 2914 метра. Разположен е в северния дял на планината и предлага спиращи дъха панорамни гледки към мраморните била и дълбоките циркуси наоколо.", false, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin-user-static-id-001", 2, "Връх Вихрен" },
                    { 3, "https://unsplash.com", "Мистична пещера в Родопите, разположена в Триградското ждрело. Известна е с огромната си зала и подземния водопад, който според легендите е пътят, по който Орфей слиза в Подземното царство.", false, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin-user-static-id-001", 3, "Дяволското гърло" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-static-id-001");
        }
    }
}
