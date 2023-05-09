using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddColumnsEquipmentMachineryOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineryType",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<string>(
                name: "DNIOperator",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "IsFrom",
                table: "EquipmentMachineryOperators",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_ProviderId",
                table: "EquipmentMachineryOperators",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators",
                column: "EquipmentMachineryTypeId",
                principalTable: "EquipmentMachineryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_Providers_ProviderId",
                table: "EquipmentMachineryOperators",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_Providers_ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "DNIOperator",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "IsFrom",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<int>(
                name: "MachineryType",
                table: "EquipmentMachineryOperators",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
