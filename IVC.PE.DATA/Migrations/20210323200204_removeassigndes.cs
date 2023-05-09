using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class removeassigndes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineryName",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "WorkArea",
                table: "EquipmentMachineryTransports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MachineryName",
                table: "EquipmentMachs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "EquipmentMachs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkArea",
                table: "EquipmentMachineryTransports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
