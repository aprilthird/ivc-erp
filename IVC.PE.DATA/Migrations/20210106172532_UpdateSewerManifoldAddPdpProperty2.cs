using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldAddPdpProperty2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDailyParts_SewerManifolds_SewerManifoldId",
                table: "ProductionDailyParts");

            migrationBuilder.DropIndex(
                name: "IX_ProductionDailyParts_SewerManifoldId",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "ProductionDailyParts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionDailyPartId",
                table: "SewerManifolds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_ProductionDailyPartId",
                table: "SewerManifolds",
                column: "ProductionDailyPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_ProductionDailyParts_ProductionDailyPartId",
                table: "SewerManifolds",
                column: "ProductionDailyPartId",
                principalTable: "ProductionDailyParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_ProductionDailyParts_ProductionDailyPartId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_ProductionDailyPartId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "ProductionDailyPartId",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "ProductionDailyParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_SewerManifoldId",
                table: "ProductionDailyParts",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDailyParts_SewerManifolds_SewerManifoldId",
                table: "ProductionDailyParts",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
