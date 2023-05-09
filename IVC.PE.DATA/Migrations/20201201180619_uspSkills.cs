using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class uspSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TuitionNumber",
                table: "Professionals");

            migrationBuilder.AddColumn<string>(
                name: "CIPNumber",
                table: "Professionals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CIPNumber",
                table: "Professionals");

            migrationBuilder.AddColumn<string>(
                name: "TuitionNumber",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
