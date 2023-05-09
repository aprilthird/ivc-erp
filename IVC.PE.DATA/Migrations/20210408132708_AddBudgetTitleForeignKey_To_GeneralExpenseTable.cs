using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBudgetTitleForeignKey_To_GeneralExpenseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "GeneralExpenses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExpenses_BudgetTitleId",
                table: "GeneralExpenses",
                column: "BudgetTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExpenses_BudgetTitles_BudgetTitleId",
                table: "GeneralExpenses",
                column: "BudgetTitleId",
                principalTable: "BudgetTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExpenses_BudgetTitles_BudgetTitleId",
                table: "GeneralExpenses");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExpenses_BudgetTitleId",
                table: "GeneralExpenses");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "GeneralExpenses");
        }
    }
}
