using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorBluePrint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Blueprints",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sheet",
                table: "Blueprints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "Sheet",
                table: "Blueprints");
        }
    }
}
