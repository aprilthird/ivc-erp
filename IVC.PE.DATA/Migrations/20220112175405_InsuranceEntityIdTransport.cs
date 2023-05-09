using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class InsuranceEntityIdTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTransportInsuranceFoldings_InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                column: "InsuranceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryTransportInsuranceFoldings_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings",
                column: "InsuranceEntityId",
                principalTable: "InsuranceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryTransportInsuranceFoldings_InsuranceEntity_InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryTransportInsuranceFoldings_InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings");

            migrationBuilder.DropColumn(
                name: "InsuranceEntityId",
                table: "EquipmentMachineryTransportInsuranceFoldings");
        }
    }
}
