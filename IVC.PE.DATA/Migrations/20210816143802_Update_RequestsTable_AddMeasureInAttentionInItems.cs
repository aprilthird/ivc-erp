using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestsTable_AddMeasureInAttentionInItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RequestDeliveryPlaceId",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAtenttion",
                table: "RequestItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MeasureInAtenttion",
                table: "PreRequestItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WarehouseId",
                table: "Requests",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Warehouses_WarehouseId",
                table: "Requests",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Warehouses_WarehouseId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_WarehouseId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MeasureInAtenttion",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "MeasureInAtenttion",
                table: "PreRequestItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestDeliveryPlaceId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
