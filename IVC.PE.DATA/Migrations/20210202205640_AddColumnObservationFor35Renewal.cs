using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnObservationFor35Renewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observation",
                table: "EquipmentCertificates");

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "EquipmentCertificateRenewals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observation",
                table: "EquipmentCertificateRenewals");

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "EquipmentCertificates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
