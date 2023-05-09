using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_DiverseInput_AddMeasureUnitId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "DiverseInputs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_MeasurementUnitId",
                table: "DiverseInputs",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiverseInputs_MeasurementUnits_MeasurementUnitId",
                table: "DiverseInputs",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiverseInputs_MeasurementUnits_MeasurementUnitId",
                table: "DiverseInputs");

            migrationBuilder.DropIndex(
                name: "IX_DiverseInputs_MeasurementUnitId",
                table: "DiverseInputs");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "DiverseInputs");
        }
    }
}
