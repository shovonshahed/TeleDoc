using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleDoc.API.Migrations
{
    public partial class addedMapLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapLocationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MapLocationId",
                table: "AspNetUsers",
                column: "MapLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MapLocations_MapLocationId",
                table: "AspNetUsers",
                column: "MapLocationId",
                principalTable: "MapLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MapLocations_MapLocationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MapLocations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MapLocationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MapLocationId",
                table: "AspNetUsers");
        }
    }
}
