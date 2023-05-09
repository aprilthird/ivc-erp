using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorProcedureProcessesEquipmentMachs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_TechnicalVersions_TechnicalVersionId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_TechnicalVersionId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "TechnicalVersionId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "LastEndDateInsurance",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastEndDateSoat",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastEndDateTechnical",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastInsuranceName",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastInsuranceNumber",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastStartDateInsurance",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastStartDateSoat",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "LastStartDateTechnical",
                table: "EquipmentMachs");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Processes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessName",
                table: "Processes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Processes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Processes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ProcessName",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Processes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TechnicalVersionId",
                table: "Procedures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateInsurance",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateSoat",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEndDateTechnical",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastInsuranceName",
                table: "EquipmentMachs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastInsuranceNumber",
                table: "EquipmentMachs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateInsurance",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateSoat",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStartDateTechnical",
                table: "EquipmentMachs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_TechnicalVersionId",
                table: "Procedures",
                column: "TechnicalVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_TechnicalVersions_TechnicalVersionId",
                table: "Procedures",
                column: "TechnicalVersionId",
                principalTable: "TechnicalVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
