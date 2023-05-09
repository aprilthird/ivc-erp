using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UserNonRelationEquipmentMachinerySoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetFormulaId",
                table: "Budgets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetTitleId",
                table: "Budgets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Budgets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Budgets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budgets",
                column: "BudgetTitleId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "BudgetFormulaId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetTitleId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Budgets");
        }
    }
}
