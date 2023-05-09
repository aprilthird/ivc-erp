using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixDataTypeFuel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AcumulatedGallon",
                table: "EquipmentMachineryFuelTransportParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Mileage",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Gallon",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AcumulatedGallon",
                table: "EquipmentMachineryFuelMachParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Horometer",
                table: "EquipmentMachineryFuelMachPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Gallon",
                table: "EquipmentMachineryFuelMachPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AcumulatedGallon",
                table: "EquipmentMachineryFuelTransportParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Mileage",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Gallon",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "AcumulatedGallon",
                table: "EquipmentMachineryFuelMachParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Horometer",
                table: "EquipmentMachineryFuelMachPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Gallon",
                table: "EquipmentMachineryFuelMachPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
