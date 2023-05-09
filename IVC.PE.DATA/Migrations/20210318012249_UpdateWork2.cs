using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWork2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachineryTransportPartFoldings");
        }
    }
}
