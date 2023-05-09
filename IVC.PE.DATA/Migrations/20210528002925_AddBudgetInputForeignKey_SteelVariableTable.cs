using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBudgetInputForeignKey_SteelVariableTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetInputId",
                table: "SteelVariables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SteelVariables_BudgetInputId",
                table: "SteelVariables",
                column: "BudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_SteelVariables_BudgetInputs_BudgetInputId",
                table: "SteelVariables",
                column: "BudgetInputId",
                principalTable: "BudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SteelVariables_BudgetInputs_BudgetInputId",
                table: "SteelVariables");

            migrationBuilder.DropIndex(
                name: "IX_SteelVariables_BudgetInputId",
                table: "SteelVariables");

            migrationBuilder.DropColumn(
                name: "BudgetInputId",
                table: "SteelVariables");
        }
    }
}
