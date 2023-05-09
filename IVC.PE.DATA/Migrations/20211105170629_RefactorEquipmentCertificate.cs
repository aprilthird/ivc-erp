using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorEquipmentCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correlative",
                table: "EquipmentCertificates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryDate",
                table: "EquipmentCertificates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalibrationFrecuency",
                table: "EquipmentCertificateRenewals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CalibrationMethod",
                table: "EquipmentCertificateRenewals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InspectionType",
                table: "EquipmentCertificateRenewals",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correlative",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "EquipmentCertificates");

            migrationBuilder.DropColumn(
                name: "CalibrationFrecuency",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "CalibrationMethod",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.DropColumn(
                name: "InspectionType",
                table: "EquipmentCertificateRenewals");
        }
    }
}
