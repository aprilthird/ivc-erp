using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class InsuranceMach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceName",
                table: "EquipmentMachInsuranceFoldings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "EquipmentMachInsuranceFoldings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "EquipmentMachInsuranceFoldings");
        }
    }
}
