using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class PhaseProjectTransportPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryTransportPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryTransportPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");
        }
    }
}
