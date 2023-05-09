using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntryItemsTable_AddOrderItemFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderItemId",
                table: "SupplyEntryItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SupplyEntryItems_OrderItemId",
                table: "SupplyEntryItems",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyEntryItems_OrderItems_OrderItemId",
                table: "SupplyEntryItems",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyEntryItems_OrderItems_OrderItemId",
                table: "SupplyEntryItems");

            migrationBuilder.DropIndex(
                name: "IX_SupplyEntryItems_OrderItemId",
                table: "SupplyEntryItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "SupplyEntryItems");
        }
    }
}
