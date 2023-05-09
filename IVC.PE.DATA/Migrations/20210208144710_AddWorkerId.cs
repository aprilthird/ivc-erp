using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddWorkerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categorytype",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerId",
                table: "EquipmentMachineryOperators",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryOperators_WorkerId",
                table: "EquipmentMachineryOperators",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryOperators_Workers_WorkerId",
                table: "EquipmentMachineryOperators",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryOperators_Workers_WorkerId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryOperators_WorkerId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "EquipmentMachineryOperators");

            migrationBuilder.AddColumn<int>(
                name: "Categorytype",
                table: "EquipmentMachineryOperators",
                type: "int",
                nullable: true);
        }
    }
}
