using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_EntibadoVariableEntity_AddNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "EntibadoVariables",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FreeDitchFondo",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FreeDitchTope",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxDitch",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Thickness",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "EntibadoVariables",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "FreeDitchFondo",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "FreeDitchTope",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "MaxDitch",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "Thickness",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "EntibadoVariables");
        }
    }
}
