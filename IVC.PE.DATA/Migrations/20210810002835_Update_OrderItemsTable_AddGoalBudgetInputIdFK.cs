using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrderItemsTable_AddGoalBudgetInputIdFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MeasurementUnits_MeasurementUnitId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Supplies_SupplyId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_MeasurementUnitId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SupplyId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UsedFor",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SupplyId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsedFor",
                table: "RequestItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplyId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MeasurementUnitId",
                table: "OrderItems",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SupplyId",
                table: "OrderItems",
                column: "SupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MeasurementUnits_MeasurementUnitId",
                table: "OrderItems",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Supplies_SupplyId",
                table: "OrderItems",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
