using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateVariablesEntities_AddBudgetInputForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetInputId",
                table: "EntibadoVariables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntibadoVariables_BudgetInputId",
                table: "EntibadoVariables",
                column: "BudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntibadoVariables_BudgetInputs_BudgetInputId",
                table: "EntibadoVariables",
                column: "BudgetInputId",
                principalTable: "BudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntibadoVariables_BudgetInputs_BudgetInputId",
                table: "EntibadoVariables");

            migrationBuilder.DropIndex(
                name: "IX_EntibadoVariables_BudgetInputId",
                table: "EntibadoVariables");

            migrationBuilder.DropColumn(
                name: "BudgetInputId",
                table: "EntibadoVariables");
        }
    }
}
