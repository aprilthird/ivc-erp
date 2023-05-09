using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_Budget_ReworkEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocationGroups_Budgets_BudgetId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Budgets_BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_MeasurementUnits_MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocationGroups_BudgetId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "GoalUnitPrice",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SaleUnitPrice",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.AddColumn<double>(
                name: "Metered",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metered",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Budgets");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetParentId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "GoalUnitPrice",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Measure",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "Budgets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SaleUnitPrice",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                table: "BudgetInputAllocationGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetParentId",
                table: "Budgets",
                column: "BudgetParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budgets",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_MeasurementUnitId",
                table: "Budgets",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocationGroups_BudgetId",
                table: "BudgetInputAllocationGroups",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocationGroups_Budgets_BudgetId",
                table: "BudgetInputAllocationGroups",
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
                name: "FK_Budgets_Budgets_BudgetParentId",
                table: "Budgets",
                column: "BudgetParentId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                table: "Budgets",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_MeasurementUnits_MeasurementUnitId",
                table: "Budgets",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
