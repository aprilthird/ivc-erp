using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollWorkerVariableHeaderIdAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollWorkerVariables_ProjectCalendarMonths_ProjectCalendarMonthId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollWorkerVariables_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarMonthId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarWeekId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarMonthId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "PayrollWorkerVariables");

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollMovementHeaderId",
                table: "PayrollWorkerVariables",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_PayrollMovementHeaderId",
                table: "PayrollWorkerVariables",
                column: "PayrollMovementHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollWorkerVariables_PayrollMovementHeaders_PayrollMovementHeaderId",
                table: "PayrollWorkerVariables",
                column: "PayrollMovementHeaderId",
                principalTable: "PayrollMovementHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollWorkerVariables_PayrollMovementHeaders_PayrollMovementHeaderId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropIndex(
                name: "IX_PayrollWorkerVariables_PayrollMovementHeaderId",
                table: "PayrollWorkerVariables");

            migrationBuilder.DropColumn(
                name: "PayrollMovementHeaderId",
                table: "PayrollWorkerVariables");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarMonthId",
                table: "PayrollWorkerVariables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "PayrollWorkerVariables",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarMonthId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarWeekId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollWorkerVariables_ProjectCalendarMonths_ProjectCalendarMonthId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarMonthId",
                principalTable: "ProjectCalendarMonths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollWorkerVariables_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
