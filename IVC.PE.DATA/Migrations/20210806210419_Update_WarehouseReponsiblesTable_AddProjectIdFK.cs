using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_WarehouseReponsiblesTable_AddProjectIdFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseResponsibles_Warehouses_WarehouseId",
                table: "WarehouseResponsibles");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseResponsibles_WarehouseId",
                table: "WarehouseResponsibles");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseResponsibles");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WarehouseResponsibles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseResponsibles_ProjectId",
                table: "WarehouseResponsibles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseResponsibles_Projects_ProjectId",
                table: "WarehouseResponsibles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseResponsibles_Projects_ProjectId",
                table: "WarehouseResponsibles");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseResponsibles_ProjectId",
                table: "WarehouseResponsibles");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WarehouseResponsibles");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "WarehouseResponsibles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseResponsibles_WarehouseId",
                table: "WarehouseResponsibles",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseResponsibles_Warehouses_WarehouseId",
                table: "WarehouseResponsibles",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
