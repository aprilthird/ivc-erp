using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProviderFoldingTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentMachineryTypeTransportId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentProviderFoldings_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings",
                column: "EquipmentMachineryTypeTransportId",
                principalTable: "EquipmentMachineryTypeTransports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentProviderFoldings_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentProviderFoldings_EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeTransportId",
                table: "EquipmentProviderFoldings");
        }
    }
}
