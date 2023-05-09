using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollMovementHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendars_ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.AddColumn<decimal>(
                name: "ConafovicerRate",
                table: "PayrollMovementHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EsSaludMasVidaCost",
                table: "PayrollMovementHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MobilityCost",
                table: "PayrollMovementHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SCTRRate",
                table: "PayrollMovementHeaders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnionFee",
                table: "PayrollMovementHeaders",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConafovicerRate",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "EsSaludMasVidaCost",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "MobilityCost",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "SCTRRate",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "UnionFee",
                table: "PayrollMovementHeaders");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarId",
                table: "PayrollMovementHeaders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendars_ProjectCalendarId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarId",
                principalTable: "ProjectCalendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
