using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestItemsTable_AddSupplyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_SupplyId",
                table: "RequestItems",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_Supplies_SupplyId",
                table: "RequestItems",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_Supplies_SupplyId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_SupplyId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "RequestItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "GoalBudgetInputId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
