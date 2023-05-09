using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorEquipmentMachSoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentSerie",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EquipmentYear",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "ServiceCondition",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceNumber",
                table: "EquipmentMachinerySofts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Potency",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerieNumber",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "EquipmentMachinerySofts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "Potency",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "SerieNumber",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AddColumn<string>(
                name: "EquipmentSerie",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentYear",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCondition",
                table: "EquipmentMachinerySofts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
