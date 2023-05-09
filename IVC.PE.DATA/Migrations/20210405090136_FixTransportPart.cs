using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixTransportPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastEndMileage",
                table: "EquipmentMachineryTransportParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastInitMileage",
                table: "EquipmentMachineryTransportParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "InitMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EndMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEndMileage",
                table: "EquipmentMachineryTransportParts");

            migrationBuilder.DropColumn(
                name: "LastInitMileage",
                table: "EquipmentMachineryTransportParts");

            migrationBuilder.AlterColumn<string>(
                name: "InitMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "EndMileage",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
