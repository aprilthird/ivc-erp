using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FromOtherOperato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromOtherDNI",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromOtherName",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromOtherPhone",
                table: "EquipmentMachineryOperators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromOtherDNI",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "FromOtherName",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "FromOtherPhone",
                table: "EquipmentMachineryOperators");
        }
    }
}
