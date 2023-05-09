using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class addmigrtioFildProfessioa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nacionality",
                table: "Professionals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Professionals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Nacionality",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "University",
                table: "Professionals");
        }
    }
}
