using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateEquipmentMachinerySoftPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachinerySoftParts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachinerySoftParts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySoftParts",
                type: "uniqueidentifier",
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
    }
}
