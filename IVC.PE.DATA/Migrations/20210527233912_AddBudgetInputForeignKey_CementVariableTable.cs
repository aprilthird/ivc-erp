using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBudgetInputForeignKey_CementVariableTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetInputId",
                table: "CementVariables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CementVariables_BudgetInputId",
                table: "CementVariables",
                column: "BudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_CementVariables_BudgetInputs_BudgetInputId",
                table: "CementVariables",
                column: "BudgetInputId",
                principalTable: "BudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CementVariables_BudgetInputs_BudgetInputId",
                table: "CementVariables");

            migrationBuilder.DropIndex(
                name: "IX_CementVariables_BudgetInputId",
                table: "CementVariables");

            migrationBuilder.DropColumn(
                name: "BudgetInputId",
                table: "CementVariables");
        }
    }
}
