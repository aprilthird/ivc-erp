using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachinerySoftUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentProviderFoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentProviderFoldings_EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentProviderFoldingId",
                principalTable: "EquipmentProviderFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentProviderFoldings_EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EquipmentProviderFoldingId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySofts",
                type: "uniqueidentifier",
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
    }
}
