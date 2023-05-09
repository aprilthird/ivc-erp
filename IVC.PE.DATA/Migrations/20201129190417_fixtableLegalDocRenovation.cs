using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixtableLegalDocRenovation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalDocumentOrder",
                table: "LegalDocumentationRenovations");

            migrationBuilder.AddColumn<int>(
                name: "LegalDocumentationOrder",
                table: "LegalDocumentationRenovations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalDocumentationOrder",
                table: "LegalDocumentationRenovations");

            migrationBuilder.AddColumn<int>(
                name: "LegalDocumentOrder",
                table: "LegalDocumentationRenovations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
