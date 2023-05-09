using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_BudgetInput_AddBudgetsForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "BudgetInputs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "BudgetInputs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTypeId",
                table: "BudgetInputs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "BudgetInputs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_BudgetFormulaId",
                table: "BudgetInputs",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_BudgetTitleId",
                table: "BudgetInputs",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_BudgetTypeId",
                table: "BudgetInputs",
                column: "BudgetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputs",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_BudgetTitles_BudgetTitleId",
                table: "BudgetInputs",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_BudgetTypes_BudgetTypeId",
                table: "BudgetInputs",
                column: "BudgetTypeId",
                principalTable: "BudgetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_BudgetTitles_BudgetTitleId",
                table: "BudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_BudgetTypes_BudgetTypeId",
                table: "BudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputs_BudgetFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputs_BudgetTitleId",
                table: "BudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputs_BudgetTypeId",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "BudgetTypeId",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "BudgetInputs");
        }
    }
}
