using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdaeColumnsEquipmentSoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EquipmentName",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateSOAT",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateInsurance",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateSOAT",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateInsurance",
                table: "EquipmentMachinerySofts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachinerySofts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateSOAT",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateInsurance",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTechnicalRevision",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateSOAT",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateInsurance",
                table: "EquipmentMachinerySofts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentName",
                table: "EquipmentMachinerySofts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
