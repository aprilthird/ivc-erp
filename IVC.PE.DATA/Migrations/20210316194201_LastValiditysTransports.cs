using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LastValiditysTransports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStartDateSoat",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStartDateInsurance",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastEndDateSoat",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastEndDateInsurance",
                table: "EquipmentMachineryTransports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastValidityInsurance",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastValiditySoat",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastValidityTechincal",
                table: "EquipmentMachineryTransports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastValidityInsurance",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastValiditySoat",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastValidityTechincal",
                table: "EquipmentMachineryTransports");

            migrationBuilder.AlterColumn<string>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastStartDateSoat",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastStartDateInsurance",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastEndDateSoat",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastEndDateInsurance",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
