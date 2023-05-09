using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Validitiys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachineryTransportSOATFoldings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachineryTransportSOATFoldings");
        }
    }
}
