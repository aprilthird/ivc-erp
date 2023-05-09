using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_PreRequestItems_AddSupplyIdAndSupplyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRequestItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "PreRequestItems");

            migrationBuilder.DropIndex(
                name: "IX_PreRequestItems_GoalBudgetInputId",
                table: "PreRequestItems");

            migrationBuilder.DropColumn(
                name: "GoalBudgetInputId",
                table: "PreRequestItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "PreRequestItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplyName",
                table: "PreRequestItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestItems_SupplyId",
                table: "PreRequestItems",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRequestItems_Supplies_SupplyId",
                table: "PreRequestItems",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreRequestItems_Supplies_SupplyId",
                table: "PreRequestItems");

            migrationBuilder.DropIndex(
                name: "IX_PreRequestItems_SupplyId",
                table: "PreRequestItems");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "PreRequestItems");

            migrationBuilder.DropColumn(
                name: "SupplyName",
                table: "PreRequestItems");

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "PreRequestItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PreRequestItems_GoalBudgetInputId",
                table: "PreRequestItems",
                column: "GoalBudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreRequestItems_GoalBudgetInputs_GoalBudgetInputId",
                table: "PreRequestItems",
                column: "GoalBudgetInputId",
                principalTable: "GoalBudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
