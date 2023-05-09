using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestItemsTable_RemoveGoalBudgetInput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "WorkFrontId",
                table: "RequestItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplyId",
                table: "RequestItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFrontId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplyId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                type: "uniqueidentifier",
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
    }
}
