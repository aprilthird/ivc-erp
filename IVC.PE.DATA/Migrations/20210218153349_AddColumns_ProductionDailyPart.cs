using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumns_ProductionDailyPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExcavatedLength",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Excavation",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FillLength",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Filled",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Filling",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GranularBaseLength",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Installation",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InstalledLength",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RefilledLength",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TheoreticalLayer",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcavatedLength",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "Excavation",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "FillLength",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "Filled",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "Filling",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "GranularBaseLength",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "Installation",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "InstalledLength",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "RefilledLength",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "TheoreticalLayer",
                table: "ProductionDailyParts");
        }
    }
}
