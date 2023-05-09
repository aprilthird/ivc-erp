using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntriesTable_Update_FieldRequestFoldings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldRequestFoldings_Supplies_SupplyId",
                table: "FieldRequestFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntries_Providers_ProviderId",
                table: "SupplyEntries");

            migrationBuilder.DropIndex(
                name: "IX_SupplyEntries_ProviderId",
                table: "SupplyEntries");

            migrationBuilder.DropIndex(
                name: "IX_FieldRequestFoldings_SupplyId",
                table: "FieldRequestFoldings");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "SupplyEntries");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "FieldRequestFoldings");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FieldRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "FieldRequestFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestFoldings_GoalBudgetInputId",
                table: "FieldRequestFoldings",
                column: "GoalBudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRequestFoldings_GoalBudgetInputs_GoalBudgetInputId",
                table: "FieldRequestFoldings",
                column: "GoalBudgetInputId",
                principalTable: "GoalBudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldRequestFoldings_GoalBudgetInputs_GoalBudgetInputId",
                table: "FieldRequestFoldings");

            migrationBuilder.DropIndex(
                name: "IX_FieldRequestFoldings_GoalBudgetInputId",
                table: "FieldRequestFoldings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FieldRequests");

            migrationBuilder.DropColumn(
                name: "GoalBudgetInputId",
                table: "FieldRequestFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "SupplyEntries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "FieldRequestFoldings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SupplyEntries_ProviderId",
                table: "SupplyEntries",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestFoldings_SupplyId",
                table: "FieldRequestFoldings",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRequestFoldings_Supplies_SupplyId",
                table: "FieldRequestFoldings",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntries_Providers_ProviderId",
                table: "SupplyEntries",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
