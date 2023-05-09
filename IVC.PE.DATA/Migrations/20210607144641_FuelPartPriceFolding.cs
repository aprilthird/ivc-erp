using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelPartPriceFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "FuelProviderPriceFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "FuelProviderPriceFoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelMachPartFoldings_FuelProviderPriceFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings",
                column: "FuelProviderPriceFoldingId",
                principalTable: "FuelProviderPriceFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelTransportPartFoldings_FuelProviderPriceFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings",
                column: "FuelProviderPriceFoldingId",
                principalTable: "FuelProviderPriceFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelMachPartFoldings_FuelProviderPriceFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelTransportPartFoldings_FuelProviderPriceFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelTransportPartFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelMachPartFoldings_FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "FuelProviderPriceFoldingId",
                table: "EquipmentMachineryFuelMachPartFoldings");
        }
    }
}
