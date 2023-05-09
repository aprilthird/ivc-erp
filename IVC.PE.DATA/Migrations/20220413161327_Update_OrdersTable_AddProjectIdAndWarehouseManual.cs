using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrdersTable_AddProjectIdAndWarehouseManual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManualWarehouse",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProjectId",
                table: "Orders",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Projects_ProjectId",
                table: "Orders",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Projects_ProjectId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProjectId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ManualWarehouse",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Orders");
        }
    }
}
