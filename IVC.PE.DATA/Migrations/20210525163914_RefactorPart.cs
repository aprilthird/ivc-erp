using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RefactorPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryTransportPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "MachineryPhaseId",
                table: "EquipmentMachPartFoldings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_MachineryPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "MachineryPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "TransportPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryTransportPartFoldings_TransportPhases_TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                column: "TransportPhaseId",
                principalTable: "TransportPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_MachineryPhases_MachineryPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "MachineryPhaseId",
                principalTable: "MachineryPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryTransportPartFoldings_TransportPhases_TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_MachineryPhases_MachineryPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_MachineryPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryTransportPartFoldings_TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "MachineryPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "TransportPhaseId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachineryTransportPartFoldings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectPhaseId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
