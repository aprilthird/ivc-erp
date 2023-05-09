using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerAddBankAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "CeaseDate",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasEPS",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasEsSaludPlusVida",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasSctr",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasUnionFee",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasWeeklySettlement",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "JudicialRetentionFixedAmmount",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "JudicialRetentionPercentRate",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "LaborRegimen",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "PensionFundAdministratorId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "PensionFundUniqueIdentificationCode",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "SctrHealthType",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "SctrPensionType",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerPositionId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Workgroup",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ForemanWorkerId1",
                table: "SewerGroups");

            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                table: "Workers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Workers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_BankId",
                table: "Workers",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups",
                column: "ForemanWorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Banks_BankId",
                table: "Workers",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Banks_BankId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_BankId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanWorkerId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "BankAccount",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Workers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CeaseDate",
                table: "Workers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasEPS",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasEsSaludPlusVida",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSctr",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasUnionFee",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWeeklySettlement",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Workers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "JudicialRetentionFixedAmmount",
                table: "Workers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "JudicialRetentionPercentRate",
                table: "Workers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LaborRegimen",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PensionFundAdministratorId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PensionFundUniqueIdentificationCode",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SctrHealthType",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SctrPensionType",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerPositionId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Workgroup",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanWorkerId1",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanWorkerId1",
                table: "SewerGroups",
                column: "ForemanWorkerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanWorkerId1",
                table: "SewerGroups",
                column: "ForemanWorkerId1",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
