using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_FieldRequestsTable_AddWorkOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldRequests_ProjectFormulas_ProjectFormulaId",
                table: "FieldRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldRequests_Warehouses_WarehouseId",
                table: "FieldRequests");

            migrationBuilder.DropIndex(
                name: "IX_FieldRequests_ProjectFormulaId",
                table: "FieldRequests");

            migrationBuilder.DropIndex(
                name: "IX_FieldRequests_WarehouseId",
                table: "FieldRequests");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "FieldRequests");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "FieldRequests");

            migrationBuilder.AddColumn<string>(
                name: "WorkOrder",
                table: "FieldRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkOrder",
                table: "FieldRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "FieldRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "FieldRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_ProjectFormulaId",
                table: "FieldRequests",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_WarehouseId",
                table: "FieldRequests",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRequests_ProjectFormulas_ProjectFormulaId",
                table: "FieldRequests",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldRequests_Warehouses_WarehouseId",
                table: "FieldRequests",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
