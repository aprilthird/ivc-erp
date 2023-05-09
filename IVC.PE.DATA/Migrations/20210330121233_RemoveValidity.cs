using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveValidity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachineryTransportSOATFoldings");

            migrationBuilder.DropColumn(
                name: "LastValidityInsurance",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastValiditySoat",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastValidityTechincal",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceName",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachineryTransportSOATFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastValidityInsurance",
                table: "EquipmentMachineryTransports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastValiditySoat",
                table: "EquipmentMachineryTransports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastValidityTechincal",
                table: "EquipmentMachineryTransports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
