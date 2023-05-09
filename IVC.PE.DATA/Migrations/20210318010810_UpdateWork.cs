using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachineryTransportPartFoldings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkArea",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkArea",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
