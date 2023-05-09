using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelProviderFoldingPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "FuelProviderFoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelTransportPartFoldings_FuelProviderFoldings_FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "FuelProviderFoldingId",
                principalTable: "FuelProviderFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelTransportPartFoldings_FuelProviderFoldings_FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "FuelProviderFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");
        }
    }
}
