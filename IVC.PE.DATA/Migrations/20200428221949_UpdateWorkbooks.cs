using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkbooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Workbooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Range",
                table: "Workbooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Workbooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Workbooks");

            migrationBuilder.DropColumn(
                name: "Range",
                table: "Workbooks");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "Workbooks");
        }
    }
}
