using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateLetterStatusKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LetterStatus",
                table: "LetterStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LetterStatus",
                table: "LetterStatus",
                columns: new[] { "LetterId", "LetterDocumentCharacteristicId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LetterStatus",
                table: "LetterStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LetterStatus",
                table: "LetterStatus",
                columns: new[] { "LetterId", "Status" });
        }
    }
}
