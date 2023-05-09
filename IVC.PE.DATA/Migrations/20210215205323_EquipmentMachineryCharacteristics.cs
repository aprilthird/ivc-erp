using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachineryCharacteristics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AssignedEquipmentMachineries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryCharacteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryCharacteristics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_ProjectId",
                table: "AssignedEquipmentMachineries",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedEquipmentMachineries_Projects_ProjectId",
                table: "AssignedEquipmentMachineries",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedEquipmentMachineries_Projects_ProjectId",
                table: "AssignedEquipmentMachineries");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_AssignedEquipmentMachineries_ProjectId",
                table: "AssignedEquipmentMachineries");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AssignedEquipmentMachineries");
        }
    }
}
