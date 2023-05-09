using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldEntityAddHasFor01HasFor47Prop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFor01",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFor47",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFor01",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "HasFor47",
                table: "SewerManifolds");
        }
    }
}
