using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ReplicEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastEndDateInsurance",
                table: "EquipmentMachineryTransports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEndDateSoat",
                table: "EquipmentMachineryTransports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachineryTransports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastStartDateInsurance",
                table: "EquipmentMachineryTransports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastStartDateSoat",
                table: "EquipmentMachineryTransports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachineryTransports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEndDateInsurance",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastEndDateSoat",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastEndDateTechnical",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastStartDateInsurance",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastStartDateSoat",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastStartDateTechnical",
                table: "EquipmentMachineryTransports");
        }
    }
}
