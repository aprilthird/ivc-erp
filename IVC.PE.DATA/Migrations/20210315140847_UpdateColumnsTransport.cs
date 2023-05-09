using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnsTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnicalOrder",
                table: "EquipmentMachineryTransportTechnicalRevisions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoatOrder",
                table: "EquipmentMachineryTransportSOATFoldings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceNumber",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoatNumber",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TechincalNumber",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderInsurance",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicalOrder",
                table: "EquipmentMachineryTransportTechnicalRevisions");

            migrationBuilder.DropColumn(
                name: "SoatOrder",
                table: "EquipmentMachineryTransportSOATFoldings");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "SoatNumber",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "TechincalNumber",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "OrderInsurance",
                table: "EquipmentMachineryTransportInsuranceFoldings");
        }
    }
}
