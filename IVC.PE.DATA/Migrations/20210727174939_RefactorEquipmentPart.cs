using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorEquipmentPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acts",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "LastEndHorometer",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "LastInitHorometer",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "OperatorName",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "Phs",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "RepStartDate",
                table: "EquipmentMachParts");

            migrationBuilder.DropColumn(
                name: "Sws",
                table: "EquipmentMachParts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Acts",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LastEndHorometer",
                table: "EquipmentMachParts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastInitHorometer",
                table: "EquipmentMachParts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OperatorName",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phs",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepStartDate",
                table: "EquipmentMachParts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sws",
                table: "EquipmentMachParts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
