using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AssignedEquipmentMach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "EquipmentMachs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachs");
        }
    }
}
