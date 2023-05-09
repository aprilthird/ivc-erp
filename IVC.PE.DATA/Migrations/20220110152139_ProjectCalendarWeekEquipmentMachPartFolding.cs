using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectCalendarWeekEquipmentMachPartFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectCalendarWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "EquipmentMachPartFoldings");
        }
    }
}
