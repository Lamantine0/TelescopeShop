using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelescopeShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Telescopes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    TripodHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    Aperture = table.Column<int>(type: "INTEGER", nullable: false),
                    Seeker = table.Column<string>(type: "TEXT", nullable: false),
                    BarlowLensIncluded = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaximumUsefulMagnification = table.Column<int>(type: "INTEGER", nullable: false),
                    FocalLength = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telescopes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Telescopes");
        }
    }
}
