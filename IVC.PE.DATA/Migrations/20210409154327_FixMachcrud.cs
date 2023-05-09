using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixMachcrud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateSoat",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateSoat",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoatNumber",
                table: "EquipmentMachs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TechincalNumber",
                table: "EquipmentMachs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEndDateSoat",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastEndDateTechnical",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastStartDateSoat",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastStartDateTechnical",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "SoatNumber",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "TechincalNumber",
                table: "EquipmentMachs");
        }
    }
}
