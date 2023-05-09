using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectCalendarFuelMach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "ProjectCalendarWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelMachPartFoldings_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "ProjectCalendarWeekId",
                principalTable: "ProjectCalendarWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelMachPartFoldings_ProjectCalendarWeeks_ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectCalendarWeekId",
                table: "EquipmentMachineryFuelMachPartFoldings");
        }
    }
}
