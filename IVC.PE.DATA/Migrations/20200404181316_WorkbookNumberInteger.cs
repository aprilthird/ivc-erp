using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkbookNumberInteger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
            //    table: "PayrollConcepts");

            //migrationBuilder.DropIndex(
            //    name: "IX_PayrollConcepts_PayrollVariableId",
            //    table: "PayrollConcepts");

            //migrationBuilder.DropColumn(
            //    name: "PayrollVariableId",
            //    table: "PayrollConcepts");

            //migrationBuilder.DropColumn(
            //    name: "PayrollConceptoId",
            //    table: "PayrollConceptFormulas");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "WorkbookSeats",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Workbooks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "PayrollConceptId",
            //    table: "PayrollConceptFormulas",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "uniqueidentifier",
            //    oldNullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "PayrollVariableId",
            //    table: "PayrollConceptFormulas",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PayrollConceptFormulas_PayrollVariableId",
            //    table: "PayrollConceptFormulas",
            //    column: "PayrollVariableId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
            //    table: "PayrollConceptFormulas",
            //    column: "PayrollVariableId",
            //    principalTable: "PayrollVariables",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PayrollConceptFormulas_PayrollVariables_PayrollVariableId",
            //    table: "PayrollConceptFormulas");

            //migrationBuilder.DropIndex(
            //    name: "IX_PayrollConceptFormulas_PayrollVariableId",
            //    table: "PayrollConceptFormulas");

            //migrationBuilder.DropColumn(
            //    name: "PayrollVariableId",
            //    table: "PayrollConceptFormulas");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "WorkbookSeats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Workbooks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "PayrollVariableId",
            //    table: "PayrollConcepts",
            //    type: "uniqueidentifier",
            //    nullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "PayrollConceptId",
            //    table: "PayrollConceptFormulas",
            //    type: "uniqueidentifier",
            //    nullable: true,
            //    oldClrType: typeof(Guid));

            //migrationBuilder.AddColumn<Guid>(
            //    name: "PayrollConceptoId",
            //    table: "PayrollConceptFormulas",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.CreateIndex(
            //    name: "IX_PayrollConcepts_PayrollVariableId",
            //    table: "PayrollConcepts",
            //    column: "PayrollVariableId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
            //    table: "PayrollConcepts",
            //    column: "PayrollVariableId",
            //    principalTable: "PayrollVariables",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
