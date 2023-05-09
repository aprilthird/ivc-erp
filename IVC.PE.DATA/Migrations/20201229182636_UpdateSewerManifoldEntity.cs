using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthBetweenAxles",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<double>(
                name: "LengthBetweenHAxles",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LengthBetweenIAxles",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthBetweenHAxles",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "LengthBetweenIAxles",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<double>(
                name: "LengthBetweenAxles",
                table: "SewerManifolds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
