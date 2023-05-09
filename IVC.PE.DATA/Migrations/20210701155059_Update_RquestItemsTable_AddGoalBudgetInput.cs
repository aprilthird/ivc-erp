using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RquestItemsTable_AddGoalBudgetInput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_GoalBudgetInputs_GoalBudgetInputId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_GoalBudgetInputId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "GoalBudgetInputId",
                table: "Requests");

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetTitleId",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_GoalBudgetInputId",
                table: "RequestItems",
                column: "GoalBudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "RequestItems",
                column: "GoalBudgetInputId",
                principalTable: "GoalBudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_GoalBudgetInputId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "GoalBudgetInputId",
                table: "RequestItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetTitleId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_GoalBudgetInputId",
                table: "Requests",
                column: "GoalBudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_GoalBudgetInputs_GoalBudgetInputId",
                table: "Requests",
                column: "GoalBudgetInputId",
                principalTable: "GoalBudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
