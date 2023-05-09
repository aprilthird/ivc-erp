using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixColumNameEquipmentCertificateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentName",
                table: "EquipmentCertificateTypes");

            migrationBuilder.AddColumn<string>(
                name: "CertificateTypeName",
                table: "EquipmentCertificateTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateTypeName",
                table: "EquipmentCertificateTypes");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentName",
                table: "EquipmentCertificateTypes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
