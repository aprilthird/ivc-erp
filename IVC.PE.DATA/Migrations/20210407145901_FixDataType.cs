using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CisternPlate",
                table: "FuelProviders");

            migrationBuilder.DropColumn(
                name: "CisternPlate2",
                table: "FuelProviders");

            migrationBuilder.AlterColumn<double>(
                name: "LastInitHorometer",
                table: "EquipmentMachParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LastEndHorometer",
                table: "EquipmentMachParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "InitHorometer",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "EndHorometer",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Dif",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LastInitMileage",
                table: "EquipmentMachineryTransportParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LastEndMileage",
                table: "EquipmentMachineryTransportParts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "InitMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "EndMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CisternPlate",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CisternPlate2",
                table: "FuelProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LastInitHorometer",
                table: "EquipmentMachParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LastEndHorometer",
                table: "EquipmentMachParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "InitHorometer",
                table: "EquipmentMachPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "EndHorometer",
                table: "EquipmentMachPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Dif",
                table: "EquipmentMachPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LastInitMileage",
                table: "EquipmentMachineryTransportParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LastEndMileage",
                table: "EquipmentMachineryTransportParts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "InitMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "EndMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
