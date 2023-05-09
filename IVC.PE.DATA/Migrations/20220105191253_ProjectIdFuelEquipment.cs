using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectIdFuelEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EquipmentMachineryFuelTransportParts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EquipmentMachineryFuelMachParts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_ProjectId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachParts_ProjectId",
                table: "EquipmentMachineryFuelMachParts",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelMachParts_Projects_ProjectId",
                table: "EquipmentMachineryFuelMachParts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelTransportParts_Projects_ProjectId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelMachParts_Projects_ProjectId",
                table: "EquipmentMachineryFuelMachParts");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelTransportParts_Projects_ProjectId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_ProjectId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelMachParts_ProjectId",
                table: "EquipmentMachineryFuelMachParts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EquipmentMachineryFuelMachParts");
        }
    }
}
