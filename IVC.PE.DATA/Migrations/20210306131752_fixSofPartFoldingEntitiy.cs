using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixSofPartFoldingEntitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specific",
                table: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specific",
                table: "EquipmentMachinerySoftPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartFoldings_EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings",
                column: "EquipmentMachineryTypeSoftActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftPartFoldings_EquipmentMachineryTypeSoftActivites_EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings",
                column: "EquipmentMachineryTypeSoftActivityId",
                principalTable: "EquipmentMachineryTypeSoftActivites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftPartFoldings_EquipmentMachineryTypeSoftActivites_EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftPartFoldings_EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartFoldings");

            migrationBuilder.DropColumn(
                name: "Specific",
                table: "EquipmentMachinerySoftPartFoldings");

            migrationBuilder.AddColumn<string>(
                name: "Specific",
                table: "EquipmentMachinerySoftPartPlusUltra",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
