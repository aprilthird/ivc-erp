using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddPayrollParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarMonths_ProjectCalendarMonthID",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarMonthID",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "ConafovicerRate",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "MobilityCost",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarMonthID",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "PayrollMovementHeaders");

            migrationBuilder.CreateTable(
                name: "PayrollParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    UIT = table.Column<decimal>(nullable: false),
                    MinimumWage = table.Column<decimal>(nullable: false),
                    DollarExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MaximumInsurableRemuneration = table.Column<decimal>(nullable: false),
                    SCTRRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    EsSaludMasVidaCost = table.Column<decimal>(nullable: false),
                    UnionFee = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollParameters_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollParameters_ProjectId",
                table: "PayrollParameters",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollParameters");

            migrationBuilder.AddColumn<decimal>(
                name: "ConafovicerRate",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MobilityCost",
                table: "PayrollMovementHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarMonthID",
                table: "PayrollMovementHeaders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarMonthID",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarMonthID");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarMonths_ProjectCalendarMonthID",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarMonthID",
                principalTable: "ProjectCalendarMonths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
