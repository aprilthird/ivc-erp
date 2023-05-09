using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectIdEquipmentMachinerySoft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EquipmentMachinerySofts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_ProjectId",
                table: "EquipmentMachinerySofts",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_Projects_ProjectId",
                table: "EquipmentMachinerySofts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_Projects_ProjectId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_ProjectId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EquipmentMachinerySofts");
        }
    }
}
