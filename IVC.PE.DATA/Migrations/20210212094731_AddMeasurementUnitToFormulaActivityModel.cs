using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddMeasurementUnitToFormulaActivityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "ProjectFormulaActivities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaActivities_MeasurementUnitId",
                table: "ProjectFormulaActivities",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFormulaActivities_MeasurementUnits_MeasurementUnitId",
                table: "ProjectFormulaActivities",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFormulaActivities_MeasurementUnits_MeasurementUnitId",
                table: "ProjectFormulaActivities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFormulaActivities_MeasurementUnitId",
                table: "ProjectFormulaActivities");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "ProjectFormulaActivities");
        }
    }
}
