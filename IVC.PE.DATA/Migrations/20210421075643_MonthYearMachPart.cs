using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class MonthYearMachPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnHo",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "InHo",
                table: "EquipmentMachParts");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "EquipmentMachParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "EquipmentMachParts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "EquipmentMachParts");

            migrationBuilder.AddColumn<string>(
                name: "EnHo",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InHo",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
