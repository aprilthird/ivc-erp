using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixMachPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoldingNumber",
                table: "EquipmentMachParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastEndHorometer",
                table: "EquipmentMachParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastInitHorometer",
                table: "EquipmentMachParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoldingNumber",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "LastEndHorometer",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "LastInitHorometer",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "order",
                table: "EquipmentMachPartFoldings");
        }
    }
}
