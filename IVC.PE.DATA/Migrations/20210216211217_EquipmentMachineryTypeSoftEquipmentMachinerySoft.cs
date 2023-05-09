using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachineryTypeSoftEquipmentMachinerySoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentMachineryTypeSoftId",
                principalTable: "EquipmentMachineryTypeSofts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts");
        }
    }
}
