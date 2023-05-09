using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixEquipmentMachinerySoftPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftParts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftParts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftParts_WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts");
        }
    }
}
