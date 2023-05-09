using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollMovementHeader2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DollarExchangeRate",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "EsSaludMasVidaCost",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "MaximumInsurableRemuneration",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "MinimumWage",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "SCTRRate",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "UIT",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "UnionFee",
                table: "PayrollMovementHeaders");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.AddColumn<decimal>(
                name: "DollarExchangeRate",
                table: "PayrollMovementHeaders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EsSaludMasVidaCost",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumInsurableRemuneration",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumWage",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SCTRRate",
                table: "PayrollMovementHeaders",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UIT",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnionFee",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
