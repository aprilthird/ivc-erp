using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SewerManifoldFor24ThirdPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PreventiveCorrectiveAction",
                table: "SewerManifoldFor24ThirdParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PreventiveCorrectiveAction",
                table: "SewerManifoldFor24ThirdParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
