using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class OperatorTransportPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_Providers_ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachineryTransportParts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachineryTransportParts");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachineryTransportPartFoldings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EquipmentMachineryTransportPartFoldings");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EquipmentMachineryTransportParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EquipmentMachineryTransportParts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "EquipmentMachineryOperators",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_ProviderId",
                table: "EquipmentMachineryOperators",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_Providers_ProviderId",
                table: "EquipmentMachineryOperators",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
