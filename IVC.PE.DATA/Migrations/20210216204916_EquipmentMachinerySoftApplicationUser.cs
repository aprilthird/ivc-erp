using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachinerySoftApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftApplicationUsers_EquipmentMachinerySofts_EquipmentMachinerySoftId",
                        column: x => x.EquipmentMachinerySoftId,
                        principalTable: "EquipmentMachinerySofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftApplicationUsers_EquipmentMachinerySoftId",
                table: "EquipmentMachinerySoftApplicationUsers",
                column: "EquipmentMachinerySoftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftApplicationUsers");
        }
    }
}
