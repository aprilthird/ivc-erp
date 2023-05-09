using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class removeInsuranceNameFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "EquipmentMachInsuranceFoldings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceName",
                table: "EquipmentMachInsuranceFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
