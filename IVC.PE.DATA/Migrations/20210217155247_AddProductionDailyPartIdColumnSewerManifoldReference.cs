using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProductionDailyPartIdColumnSewerManifoldReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionDailyPartId",
                table: "SewerManifoldReferences",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldReferences_ProductionDailyPartId",
                table: "SewerManifoldReferences",
                column: "ProductionDailyPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldReferences_ProductionDailyParts_ProductionDailyPartId",
                table: "SewerManifoldReferences",
                column: "ProductionDailyPartId",
                principalTable: "ProductionDailyParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldReferences_ProductionDailyParts_ProductionDailyPartId",
                table: "SewerManifoldReferences");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldReferences_ProductionDailyPartId",
                table: "SewerManifoldReferences");

            migrationBuilder.DropColumn(
                name: "ProductionDailyPartId",
                table: "SewerManifoldReferences");
        }
    }
}
