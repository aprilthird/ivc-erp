using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrolls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {   /*
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementDetails_PayrollVariables_PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendars_ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PayrollMovementDetails_PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.DropIndex(
                name: "IX_PayrollConcepts_PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarId",
                table: "PayrollMovementHeaders");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollMovementDetails");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.DropColumn(
                name: "PayrollConceptoId",
                table: "PayrollConceptFormulas");

            migrationBuilder.AddColumn<decimal>(
                name: "DisabilityInsuranceRate",
                table: "PayrollPensionFundAdministratorRates",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollConceptFormulas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayrollWorkerVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    PayrollVariableId = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: true),
                    ProjectCalendarMonthId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(nullable: true)
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
                name: "IX_PayrollConceptFormulas_PayrollVariableId",
                table: "PayrollConceptFormulas",
                column: "PayrollVariableId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
                table: "PayrollConceptFormulas",
                column: "PayrollVariableId",
                principalTable: "PayrollVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {/*
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.DropTable(
                name: "PayrollWorkerVariables");

            migrationBuilder.DropIndex(
                name: "IX_PayrollConceptFormulas_PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.DropColumn(
                name: "DisabilityInsuranceRate",
                table: "PayrollPensionFundAdministratorRates");

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

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PayrollConcepts");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarId",
                table: "PayrollMovementHeaders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollMovementDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollConcepts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PayrollConceptId",
                table: "PayrollConceptFormulas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollConceptoId",
                table: "PayrollConceptFormulas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_PayrollVariableId",
                table: "PayrollMovementDetails",
                column: "PayrollVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollConcepts_PayrollVariableId",
                table: "PayrollConcepts",
                column: "PayrollVariableId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
                table: "PayrollConcepts",
                column: "PayrollVariableId",
                principalTable: "PayrollVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementDetails_PayrollVariables_PayrollVariableId",
                table: "PayrollMovementDetails",
                column: "PayrollVariableId",
                principalTable: "PayrollVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollMovementHeaders_ProjectCalendars_ProjectCalendarId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarId",
                principalTable: "ProjectCalendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }
    }
}
