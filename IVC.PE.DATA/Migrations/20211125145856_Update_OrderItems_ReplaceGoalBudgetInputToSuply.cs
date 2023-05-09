using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrderItems_ReplaceGoalBudgetInputToSuply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_GoalBudgetInputId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "GoalBudgetInputId",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "OrderItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SupplyId",
                table: "OrderItems",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Supplies_SupplyId",
                table: "OrderItems",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Supplies_SupplyId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SupplyId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_GoalBudgetInputId",
                table: "OrderItems",
                column: "GoalBudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "OrderItems",
                column: "GoalBudgetInputId",
                principalTable: "GoalBudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
