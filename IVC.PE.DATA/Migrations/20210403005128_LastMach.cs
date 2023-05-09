using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LastMach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastInsuranceName",
                table: "EquipmentMachs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastInsuranceNumber",
                table: "EquipmentMachs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastInsuranceName",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastInsuranceNumber",
                table: "EquipmentMachs");
        }
    }
}
