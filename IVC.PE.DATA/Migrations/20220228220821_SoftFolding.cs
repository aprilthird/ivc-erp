using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SoftFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreeText",
                table: "EquipmentMachinerySofts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftId = table.Column<Guid>(nullable: false),
                    FreeText = table.Column<string>(nullable: true),
                    FreeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftFoldings_EquipmentMachinerySofts_EquipmentMachinerySoftId",
                        column: x => x.EquipmentMachinerySoftId,
                        principalTable: "EquipmentMachinerySofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftFoldings_EquipmentMachinerySoftId",
                table: "EquipmentMachinerySoftFoldings",
                column: "EquipmentMachinerySoftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftFoldings");

            migrationBuilder.DropColumn(
                name: "FreeText",
                table: "EquipmentMachinerySofts");
        }
    }
}
