using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class removecolumnequipmentcertificate_addcolumnrenewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificateRenewals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentCertificateNumber",
                table: "EquipmentCertificates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
