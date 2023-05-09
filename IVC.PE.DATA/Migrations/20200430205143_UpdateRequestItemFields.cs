using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRequestItemFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BudgetInputId",
                table: "RequestItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_BudgetInputId",
                table: "RequestItems",
                column: "BudgetInputId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_BudgetInputs_BudgetInputId",
                table: "RequestItems",
                column: "BudgetInputId",
                principalTable: "BudgetInputs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_BudgetInputs_BudgetInputId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_BudgetInputId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "BudgetInputId",
                table: "RequestItems");
        }
    }
}
