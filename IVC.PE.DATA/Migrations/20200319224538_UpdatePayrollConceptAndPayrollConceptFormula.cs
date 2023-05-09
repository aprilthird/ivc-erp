using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollConceptAndPayrollConceptFormula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.DropIndex(
                name: "IX_PayrollConcepts_PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollConcepts");

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollConceptFormulas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollConceptFormulas_PayrollVariableId",
                table: "PayrollConceptFormulas",
                column: "PayrollVariableId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
                table: "PayrollConceptFormulas",
                column: "PayrollVariableId",
                principalTable: "PayrollVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.DropIndex(
                name: "IX_PayrollConceptFormulas_PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.DropColumn(
                name: "PayrollVariableId",
                table: "PayrollConceptFormulas");

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollVariableId",
                table: "PayrollConcepts",
                type: "uniqueidentifier",
                nullable: true);

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
        }
    }
}
