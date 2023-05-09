using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddOptionalForeignKey_BudgetTypeId_To_Budget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTypeId",
                table: "Budgets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTypeId",
                table: "Budgets",
                column: "BudgetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetTypes_BudgetTypeId",
                table: "Budgets",
                column: "BudgetTypeId",
                principalTable: "BudgetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetTypes_BudgetTypeId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetTypeId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetTypeId",
                table: "Budgets");
        }
    }
}
