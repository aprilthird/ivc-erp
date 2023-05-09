using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class OperatorTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeTransportId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeTransportId",
                principalTable: "EquipmentMachineryTypeTransports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypeTransports_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeTransportId",
                table: "EquipmentMachineryOperators");
        }
    }
}
