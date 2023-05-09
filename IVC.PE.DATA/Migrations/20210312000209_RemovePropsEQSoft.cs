using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemovePropsEQSoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateInsurance",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EndDateSOAT",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EndDateTechnicalRevision",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "InsuranceFileUrl",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "InsuranceSOATFileUrl",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "InsuranceTechnicalRevisionFileUrl",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "StartDateInsurance",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "StartDateSOAT",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "StartDateTechnicalRevision",
                table: "EquipmentMachinerySofts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateInsurance",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateSOAT",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceFileUrl",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceSOATFileUrl",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceTechnicalRevisionFileUrl",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateInsurance",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateSOAT",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: true);
        }
    }
}
