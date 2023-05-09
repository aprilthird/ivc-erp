using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollAndAddPayrollWorkerVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementDetails_PayrollVariables_PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementDetails_PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "DisabilityInsuranceRate",
                table: "PayrollPensionFundAdministratorRates",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PayrollConcepts",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PayrollConceptId",
                table: "PayrollConceptFormulas",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PayrollWorkerVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    PayrollVariableId = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: true),
                    ProjectCalendarMonthId = table.Column<Guid>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollWorkerVariables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollWorkerVariables_PayrollVariables_PayrollVariableId",
                        column: x => x.PayrollVariableId,
                        principalTable: "PayrollVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollWorkerVariables_ProjectCalendarMonths_ProjectCalendarMonthId",
                        column: x => x.ProjectCalendarMonthId,
                        principalTable: "ProjectCalendarMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollWorkerVariables_ProjectCalendarWeeks_ProjectCalendarWeekId",
                        column: x => x.ProjectCalendarWeekId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollWorkerVariables_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_PayrollVariableId",
                table: "PayrollWorkerVariables",
                column: "PayrollVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarMonthId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_ProjectCalendarWeekId",
                table: "PayrollWorkerVariables",
                column: "ProjectCalendarWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerVariables_WorkerId",
                table: "PayrollWorkerVariables",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollWorkerVariables");

            migrationBuilder.DropColumn(
                name: "DisabilityInsuranceRate",
                table: "PayrollPensionFundAdministratorRates");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PayrollConcepts");

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollMovementDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PayrollConceptId",
                table: "PayrollConceptFormulas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_PayrollVariableId",
                table: "PayrollMovementDetails",
                column: "PayrollVariableId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementDetails_PayrollVariables_PayrollVariableId",
                table: "PayrollMovementDetails",
                column: "PayrollVariableId",
                principalTable: "PayrollVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
