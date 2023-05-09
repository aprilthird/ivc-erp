using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DeleteBudgetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_Budgets_BudgetId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "Budget");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budget",
                newName: "IX_Budget_BudgetTitleId");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budget",
                newName: "IX_Budget_BudgetFormulaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budget",
                table: "Budget",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetFormulas_BudgetFormulaId",
                table: "Budget",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetTitles_BudgetTitleId",
                table: "Budget",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocations_Budget_BudgetId",
                table: "BudgetInputAllocations",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetFormulas_BudgetFormulaId",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetTitles_BudgetTitleId",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_Budget_BudgetId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budget",
                table: "Budget");

            migrationBuilder.RenameTable(
                name: "Budget",
                newName: "Budgets");

            migrationBuilder.RenameIndex(
                name: "IX_Budget_BudgetTitleId",
                table: "Budgets",
                newName: "IX_Budgets_BudgetTitleId");

            migrationBuilder.RenameIndex(
                name: "IX_Budget_BudgetFormulaId",
                table: "Budgets",
                newName: "IX_Budgets_BudgetFormulaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocations_Budgets_BudgetId",
                table: "BudgetInputAllocations",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                table: "Budgets",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                table: "Budgets",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
