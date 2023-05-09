using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocationGroups_BudgetInputAllocations_BudgetId_BudgetInputId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetInputAllocations",
                table: "BudgetInputAllocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetInputAllocationGroups",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropColumn(
                name: "GoalAmmount",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SaleAmmount",
                table: "Budgets");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetParentId",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GoalUnitPrice",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Measure",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberItem",
                table: "Budgets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SaleUnitPrice",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BudgetInputAllocations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "BudgetInputAllocations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "BudgetInputAllocations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "BudgetInputAllocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "BudgetInputAllocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetInputId",
                table: "BudgetInputAllocationGroups",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetId",
                table: "BudgetInputAllocationGroups",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetInputAllocationId",
                table: "BudgetInputAllocationGroups",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetInputAllocations",
                table: "BudgetInputAllocations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetInputAllocationGroups",
                table: "BudgetInputAllocationGroups",
                columns: new[] { "BudgetInputAllocationId", "SewerGroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetParentId",
                table: "Budgets",
                column: "BudgetParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_MeasurementUnitId",
                table: "Budgets",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocations_BudgetFormulaId",
                table: "BudgetInputAllocations",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocations_BudgetId",
                table: "BudgetInputAllocations",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocations_BudgetTitleId",
                table: "BudgetInputAllocations",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocationGroups_BudgetId",
                table: "BudgetInputAllocationGroups",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocationGroups_BudgetInputAllocations_BudgetInputAllocationId",
                table: "BudgetInputAllocationGroups",
                column: "BudgetInputAllocationId",
                principalTable: "BudgetInputAllocations",
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
                name: "FK_BudgetInputAllocations_BudgetTitles_BudgetTitleId",
                table: "BudgetInputAllocations",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
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
                name: "FK_Budgets_MeasurementUnits_MeasurementUnitId",
                table: "Budgets",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocationGroups_BudgetInputAllocations_BudgetInputAllocationId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_BudgetFormulas_BudgetFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetInputAllocations_BudgetTitles_BudgetTitleId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Budgets_BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_MeasurementUnits_MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetInputAllocations",
                table: "BudgetInputAllocations");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocations_BudgetFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocations_BudgetId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocations_BudgetTitleId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetInputAllocationGroups",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropIndex(
                name: "IX_BudgetInputAllocationGroups_BudgetId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.DropColumn(
                name: "BudgetParentId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "GoalUnitPrice",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "NumberItem",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SaleUnitPrice",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "BudgetInputAllocations");

            migrationBuilder.DropColumn(
                name: "BudgetInputAllocationId",
                table: "BudgetInputAllocationGroups");

            migrationBuilder.AddColumn<double>(
                name: "GoalAmmount",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SaleAmmount",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetInputId",
                table: "BudgetInputAllocationGroups",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetId",
                table: "BudgetInputAllocationGroups",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetInputAllocations",
                table: "BudgetInputAllocations",
                columns: new[] { "BudgetId", "BudgetInputId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetInputAllocationGroups",
                table: "BudgetInputAllocationGroups",
                columns: new[] { "BudgetId", "BudgetInputId", "SewerGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetInputAllocationGroups_BudgetInputAllocations_BudgetId_BudgetInputId",
                table: "BudgetInputAllocationGroups",
                columns: new[] { "BudgetId", "BudgetInputId" },
                principalTable: "BudgetInputAllocations",
                principalColumns: new[] { "BudgetId", "BudgetInputId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
