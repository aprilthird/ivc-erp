using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnBziTTAndBzjTT_ToSewerManifoldFor47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BZiRealTerrainType",
                table: "SewerManifoldFor47s",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BZjRealTerrainType",
                table: "SewerManifoldFor47s",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BZiRealTerrainType",
                table: "SewerManifoldFor47s");

            migrationBuilder.DropColumn(
                name: "BZjRealTerrainType",
                table: "SewerManifoldFor47s");
        }
    }
}
