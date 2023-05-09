using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_GoalBudgetInput_ReWorkColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_BudgetTypes_BudgetTypeId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_Projects_ProjectId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_BudgetFormulaId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_BudgetTypeId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_ProjectId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "BudgetTypeId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "GoalUnitPrice",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "SaleUnitPrice",
                table: "GoalBudgetInputs");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_SupplyId",
                table: "GoalBudgetInputs",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_Supplies_SupplyId",
                table: "GoalBudgetInputs",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_Supplies_SupplyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_SupplyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "GoalBudgetInputs");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "GoalBudgetInputs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTypeId",
                table: "GoalBudgetInputs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "GoalBudgetInputs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GoalBudgetInputs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GoalUnitPrice",
                table: "GoalBudgetInputs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "GoalBudgetInputs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "GoalBudgetInputs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "SaleUnitPrice",
                table: "GoalBudgetInputs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_BudgetFormulaId",
                table: "GoalBudgetInputs",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_BudgetTypeId",
                table: "GoalBudgetInputs",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_ProjectId",
                table: "GoalBudgetInputs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_BudgetFormulas_BudgetFormulaId",
                table: "GoalBudgetInputs",
                column: "BudgetFormulaId",
                principalTable: "BudgetFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_BudgetTypes_BudgetTypeId",
                table: "GoalBudgetInputs",
                column: "BudgetTypeId",
                principalTable: "BudgetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_Projects_ProjectId",
                table: "GoalBudgetInputs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
