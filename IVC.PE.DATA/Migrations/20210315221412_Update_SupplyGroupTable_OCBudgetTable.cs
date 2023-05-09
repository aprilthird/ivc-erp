using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyGroupTable_OCBudgetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OCBudgets_BudgetGroups_BudgetGroupId",
                table: "OCBudgets");

            migrationBuilder.DropIndex(
                name: "IX_OCBudgets_BudgetGroupId",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "BudgetGroupId",
                table: "OCBudgets");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "SupplyGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "OCBudgets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyGroups_SupplyFamilyId",
                table: "SupplyGroups",
                column: "SupplyFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyGroups_SupplyFamilies_SupplyFamilyId",
                table: "SupplyGroups",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGroups_SupplyFamilies_SupplyFamilyId",
                table: "SupplyGroups");

            migrationBuilder.DropIndex(
                name: "IX_SupplyGroups_SupplyFamilyId",
                table: "SupplyGroups");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "SupplyGroups");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "OCBudgets");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetGroupId",
                table: "OCBudgets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_BudgetGroupId",
                table: "OCBudgets",
                column: "BudgetGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_OCBudgets_BudgetGroups_BudgetGroupId",
                table: "OCBudgets",
                column: "BudgetGroupId",
                principalTable: "BudgetGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
