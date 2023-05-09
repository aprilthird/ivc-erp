using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentTransportRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "LastInsuranceName",
                table: "EquipmentMachineryTransports");

            migrationBuilder.DropColumn(
                name: "LastInsuranceNumber",
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

            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "EquipmentMachineryTransportInsuranceFoldings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateInsurance",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateSoat",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastInsuranceName",
                table: "EquipmentMachineryTransports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastInsuranceNumber",
                table: "EquipmentMachineryTransports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateInsurance",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateSoat",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachineryTransports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceName",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
