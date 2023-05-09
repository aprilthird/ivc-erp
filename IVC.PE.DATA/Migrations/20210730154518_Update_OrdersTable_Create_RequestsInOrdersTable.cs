using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrdersTable_Create_RequestsInOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetFormulas_Projects_ProjectId",
                table: "BudgetFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesUtilities_BudgetFormulas_BudgetFormulaId",
                table: "ExpensesUtilities");

            migrationBuilder.DropForeignKey(
                name: "FK_OCBudgets_BudgetFormulas_BudgetFormulaId",
                table: "OCBudgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetFormulas",
                table: "BudgetFormulas");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "BudgetFormulas",
                newName: "BudgetFormula");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetFormulas_ProjectId",
                table: "BudgetFormula",
                newName: "IX_BudgetFormula_ProjectId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectFormulaId",
                table: "Budgets",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetFormula",
                table: "BudgetFormula",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RequestsInOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    RequestId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsInOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestsInOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestsInOrders_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestsInOrders_OrderId",
                table: "RequestsInOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestsInOrders_RequestId",
                table: "RequestsInOrders",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetFormula_Projects_ProjectId",
                table: "BudgetFormula",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocations_BudgetFormula_BudgetFormulaId",
                table: "BudgetInputAllocations",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_BudgetFormula_BudgetFormulaId",
                table: "BudgetInputs",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensesUtilities_BudgetFormula_BudgetFormulaId",
                table: "ExpensesUtilities",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCBudgets_BudgetFormula_BudgetFormulaId",
                table: "OCBudgets",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetFormula_Projects_ProjectId",
                table: "BudgetFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_BudgetFormula_BudgetFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputs_BudgetFormula_BudgetFormulaId",
                table: "BudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesUtilities_BudgetFormula_BudgetFormulaId",
                table: "ExpensesUtilities");

            migrationBuilder.DropForeignKey(
                name: "FK_OCBudgets_BudgetFormula_BudgetFormulaId",
                table: "OCBudgets");

            migrationBuilder.DropTable(
                name: "RequestsInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetFormula",
                table: "BudgetFormula");

            migrationBuilder.RenameTable(
                name: "BudgetFormula",
                newName: "BudgetFormulas");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetFormula_ProjectId",
                table: "BudgetFormulas",
                newName: "IX_BudgetFormulas_ProjectId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectFormulaId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetFormulas",
                table: "BudgetFormulas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets",
                column: "BudgetFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetFormulas_Projects_ProjectId",
                table: "BudgetFormulas",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocations_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputAllocations",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputs",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
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
                name: "FK_ExpensesUtilities_BudgetFormulas_BudgetFormulaId",
                table: "ExpensesUtilities",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCBudgets_BudgetFormulas_BudgetFormulaId",
                table: "OCBudgets",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
