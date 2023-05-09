using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveMachValidity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastValidityInsurance",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "EquipmentMachInsuranceFoldings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastValidityInsurance",
                table: "EquipmentMachs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "EquipmentMachInsuranceFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
