using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateUserColumncs_SewerManifoldFor24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportUser",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ResponsableUser",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportUserId",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableUserId",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ReportUserId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ResponsableUserId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<string>(
                name: "ReportUser",
                table: "SewerManifoldFor24FirstParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableUser",
                table: "SewerManifoldFor24FirstParts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
