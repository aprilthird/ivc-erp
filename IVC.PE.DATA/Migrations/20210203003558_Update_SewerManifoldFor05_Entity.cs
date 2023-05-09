using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SewerManifoldFor05_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Filling",
                table: "SewerManifoldFor05s",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "LayersNumber",
                table: "SewerManifoldFor05s",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filling",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "LayersNumber",
                table: "SewerManifoldFor05s");
        }
    }
}
