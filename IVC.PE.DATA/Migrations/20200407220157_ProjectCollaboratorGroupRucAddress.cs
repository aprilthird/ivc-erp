using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectCollaboratorGroupRucAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ProjectCollaboratorGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RUC",
                table: "ProjectCollaboratorGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ProjectCollaboratorGroups");

            migrationBuilder.DropColumn(
                name: "RUC",
                table: "ProjectCollaboratorGroups");
        }
    }
}
