using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelescopeShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTelescopePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Telescopes",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 24990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 21990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 39990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 129990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 74990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 34990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Price",
                value: 11990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Price",
                value: 18990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Price",
                value: 89990m);

            migrationBuilder.UpdateData(
                table: "Telescopes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Price",
                value: 54990m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Telescopes");
        }
    }
}
