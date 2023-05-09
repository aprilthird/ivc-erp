using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBoolsDayEquipmentMachinery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachTechnicalRevisions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachTechnicalRevisions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachSOATFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachSOATFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachInsuranceFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachInsuranceFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachineryTransportSOATFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachineryTransportSOATFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachSOATFoldings");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachSOATFoldings");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachineryTransportSOATFoldings");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachineryTransportSOATFoldings");

            migrationBuilder.DropColumn(
                name: "Days15",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "EquipmentMachineryTransportInsuranceFoldings");
        }
    }
}
