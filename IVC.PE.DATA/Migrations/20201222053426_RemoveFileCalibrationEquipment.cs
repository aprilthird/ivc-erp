using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveFileCalibrationEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileCalibration",
                table: "EquipmentCertificateRenewals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileCalibration",
                table: "EquipmentCertificateRenewals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
