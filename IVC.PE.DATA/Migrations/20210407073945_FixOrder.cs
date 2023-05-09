using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "order",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                newName: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Order",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                newName: "order");

            migrationBuilder.AlterColumn<string>(
                name: "order",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
