using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TelescopeShop_.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTelescopes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Telescopes",
                columns: new[] { "Id", "Aperture", "BarlowLensIncluded", "FocalLength", "Manufacturer", "MaximumUsefulMagnification", "Seeker", "TripodHeight" },
                values: new object[,]
                {
                    { 1, 130, false, 650, "Celestron", 260, "Red dot finder", 1200 },
                    { 2, 130, false, 650, "Sky-Watcher", 260, "Red dot finder", 1000 },
                    { 3, 203, false, 1200, "Orion", 406, "EZ Finder II", 1200 },
                    { 4, 203, false, 2032, "Meade", 406, "8x50 finder", 1300 },
                    { 5, 150, false, 1500, "Celestron", 300, "StarPointer red dot finder", 1200 },
                    { 6, 200, false, 1200, "Sky-Watcher", 400, "8x50 finder", 1250 },
                    { 7, 70, true, 700, "Celestron", 140, "5x24 finder", 1000 },
                    { 8, 114, false, 450, "Orion", 228, "EZ Finder II", 900 },
                    { 9, 254, false, 1250, "GSO", 508, "8x50 finder", 1300 },
                    { 10, 102, false, 1325, "Celestron", 204, "StarPointer red dot finder", 1100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
