using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestsTable_AddGoalBudgetInputId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "BudgetTitleId",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "Requests",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
