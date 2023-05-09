using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveMachineryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropColumn(
                name: "AcumulatedMileage",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts");

            migrationBuilder.DropColumn(
                name: "RateConsume",
                table: "EquipmentMachineryFuelTransportParts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcumulatedMileage",
                table: "EquipmentMachineryFuelTransportParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RateConsume",
                table: "EquipmentMachineryFuelTransportParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryFuelTransportParts_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentMachineryFuelTransportParts",
                column: "EquipmentMachineryTypeId",
                principalTable: "EquipmentMachineryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
