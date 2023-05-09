using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveColumnProjectEquipmentMachineryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachineryTypes_Projects_ProjectId",
                table: "EquipmentMachineryTypes");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachineryTypes_ProjectId",
                table: "EquipmentMachineryTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EquipmentMachineryTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EquipmentMachineryTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTypes_ProjectId",
                table: "EquipmentMachineryTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachineryTypes_Projects_ProjectId",
                table: "EquipmentMachineryTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
