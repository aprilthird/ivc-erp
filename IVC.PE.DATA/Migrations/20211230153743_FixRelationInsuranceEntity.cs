using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixRelationInsuranceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachs_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachs");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachs_InsuranceEntityId",
                table: "EquipmentMachs");

            migrationBuilder.DropColumn(
                name: "InsuranceEntityId",
                table: "EquipmentMachs");

            migrationBuilder.AddColumn<Guid>(
                name: "InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachInsuranceFoldings_InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings",
                column: "InsuranceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachInsuranceFoldings_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings",
                column: "InsuranceEntityId",
                principalTable: "InsuranceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachInsuranceFoldings_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachInsuranceFoldings_InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "InsuranceEntityId",
                table: "EquipmentMachInsuranceFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "InsuranceEntityId",
                table: "EquipmentMachs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachs_InsuranceEntityId",
                table: "EquipmentMachs",
                column: "InsuranceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachs_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachs",
                column: "InsuranceEntityId",
                principalTable: "InsuranceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
