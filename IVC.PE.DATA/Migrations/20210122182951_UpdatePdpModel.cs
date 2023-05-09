using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePdpModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectFormula",
                table: "ProductionDailyParts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "ProductionDailyParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_ProjectFormulaId",
                table: "ProductionDailyParts",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDailyParts_ProjectFormulas_ProjectFormulaId",
                table: "ProductionDailyParts",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDailyParts_ProjectFormulas_ProjectFormulaId",
                table: "ProductionDailyParts");

            migrationBuilder.DropIndex(
                name: "IX_ProductionDailyParts_ProjectFormulaId",
                table: "ProductionDailyParts");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "ProductionDailyParts");

            migrationBuilder.AddColumn<int>(
                name: "ProjectFormula",
                table: "ProductionDailyParts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
