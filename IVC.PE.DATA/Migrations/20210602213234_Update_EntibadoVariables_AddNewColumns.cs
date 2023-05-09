using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_EntibadoVariables_AddNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HDimension",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LDimension",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RestatedPerformance",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UseFactor",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HDimension",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "LDimension",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "RestatedPerformance",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "UseFactor",
                table: "EntibadoVariables");
        }
    }
}
