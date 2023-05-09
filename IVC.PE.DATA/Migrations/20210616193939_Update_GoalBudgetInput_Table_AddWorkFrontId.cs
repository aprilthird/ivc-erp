using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_GoalBudgetInput_Table_AddWorkFrontId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_SupplyFamilies_SupplyFamilyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_SupplyGroups_SupplyGroupId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_SupplyFamilyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_SupplyGroupId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "SupplyFamilyId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "SupplyGroupId",
                table: "GoalBudgetInputs");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_WorkFrontId",
                table: "GoalBudgetInputs",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_WorkFronts_WorkFrontId",
                table: "GoalBudgetInputs",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalBudgetInputs_WorkFronts_WorkFrontId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropIndex(
                name: "IX_GoalBudgetInputs_WorkFrontId",
                table: "GoalBudgetInputs");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "GoalBudgetInputs");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyFamilyId",
                table: "GoalBudgetInputs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyGroupId",
                table: "GoalBudgetInputs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_SupplyFamilyId",
                table: "GoalBudgetInputs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_SupplyGroupId",
                table: "GoalBudgetInputs",
                column: "SupplyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_SupplyFamilies_SupplyFamilyId",
                table: "GoalBudgetInputs",
                column: "SupplyFamilyId",
                principalTable: "SupplyFamilies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalBudgetInputs_SupplyGroups_SupplyGroupId",
                table: "GoalBudgetInputs",
                column: "SupplyGroupId",
                principalTable: "SupplyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
