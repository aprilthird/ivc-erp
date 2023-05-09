using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TempRemoveColumnHasAVoid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAVoid",
                table: "EquipmentCertificateRenewals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAVoid",
                table: "EquipmentCertificateRenewals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
