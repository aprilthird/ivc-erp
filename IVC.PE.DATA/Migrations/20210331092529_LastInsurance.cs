using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LastInsurance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastInsuranceName",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastInsuranceNumber",
                table: "EquipmentMachineryTransports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastInsuranceName",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastInsuranceNumber",
                table: "EquipmentMachineryTransports");
        }
    }
}
