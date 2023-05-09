using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SoftPartLast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftParts_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftParts_ProjectPhaseId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachinerySoftParts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachinerySoftParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_ProjectPhaseId",
                table: "EquipmentMachinerySoftParts",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftParts_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachinerySoftParts",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
