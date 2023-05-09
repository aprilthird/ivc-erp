using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixProblems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfFoldings",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "EquipmentMachinerySoftPartFoldings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfFoldings",
                table: "EquipmentMachinerySoftParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "EquipmentMachinerySoftPartFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
