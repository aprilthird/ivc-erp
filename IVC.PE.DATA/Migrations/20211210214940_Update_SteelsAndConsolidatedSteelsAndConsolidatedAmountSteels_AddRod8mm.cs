using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SteelsAndConsolidatedSteelsAndConsolidatedAmountSteels_AddRod8mm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rod8mm",
                table: "Steels",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rod8mm",
                table: "ConsolidatedSteels",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Rod8mm",
                table: "ConsolidatedAmountSteels",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rod8mm",
                table: "Steels");

            migrationBuilder.DropColumn(
                name: "Rod8mm",
                table: "ConsolidatedSteels");

            migrationBuilder.DropColumn(
                name: "Rod8mm",
                table: "ConsolidatedAmountSteels");
        }
    }
}
