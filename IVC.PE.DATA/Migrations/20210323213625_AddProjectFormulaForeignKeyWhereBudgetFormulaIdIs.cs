using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectFormulaForeignKeyWhereBudgetFormulaIdIs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "OCBudgets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "ExpensesUtilities",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "BudgetInputs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "BudgetInputAllocations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_ProjectFormulaId",
                table: "OCBudgets",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesUtilities_ProjectFormulaId",
                table: "ExpensesUtilities",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_ProjectFormulaId",
                table: "Budgets",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_ProjectFormulaId",
                table: "BudgetInputs",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocations_ProjectFormulaId",
                table: "BudgetInputAllocations",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocations_ProjectFormulas_ProjectFormulaId",
                table: "BudgetInputAllocations",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_ProjectFormulas_ProjectFormulaId",
                table: "BudgetInputs",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_ProjectFormulas_ProjectFormulaId",
                table: "Budgets",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensesUtilities_ProjectFormulas_ProjectFormulaId",
                table: "ExpensesUtilities",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCBudgets_ProjectFormulas_ProjectFormulaId",
                table: "OCBudgets",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_ProjectFormulas_ProjectFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_ProjectFormulas_ProjectFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_ProjectFormulas_ProjectFormulaId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesUtilities_ProjectFormulas_ProjectFormulaId",
                table: "ExpensesUtilities");

            migrationBuilder.DropForeignKey(
                name: "FK_OCBudgets_ProjectFormulas_ProjectFormulaId",
                table: "OCBudgets");

            migrationBuilder.DropIndex(
                name: "IX_OCBudgets_ProjectFormulaId",
                table: "OCBudgets");

            migrationBuilder.DropIndex(
                name: "IX_ExpensesUtilities_ProjectFormulaId",
                table: "ExpensesUtilities");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_ProjectFormulaId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputs_ProjectFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocations_ProjectFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "ExpensesUtilities");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "BudgetInputAllocations");
        }
    }
}
