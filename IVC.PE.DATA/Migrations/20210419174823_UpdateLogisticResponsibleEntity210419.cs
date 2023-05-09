using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateLogisticResponsibleEntity210419 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "RequestUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "LogisticResponsibles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "LogisticResponsibles");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "RequestUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
