using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntriesTable_AddRemissionGuideUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemissionGuide",
                table: "SupplyEntries");

            migrationBuilder.AddColumn<string>(
                name: "RemissionGuideUrl",
                table: "SupplyEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemissionGuideUrl",
                table: "SupplyEntries");

            migrationBuilder.AddColumn<string>(
                name: "RemissionGuide",
                table: "SupplyEntries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
