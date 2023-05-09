using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class OrderFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoldingNumber",
                table: "EquipmentMachineryFuelTransportParts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "order",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoldingNumber",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropColumn(
                name: "order",
                table: "EquipmentMachineryFuelTransportPartFoldings");
        }
    }
}
