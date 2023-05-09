using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class MachApplicationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachInsuranceFoldingApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachInsuranceFoldingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachInsuranceFoldingApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachInsuranceFoldingApplicationUsers_EquipmentMachInsuranceFoldings_EquipmentMachInsuranceFoldingId",
                        column: x => x.EquipmentMachInsuranceFoldingId,
                        principalTable: "EquipmentMachInsuranceFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachInsuranceFoldingApplicationUsers_EquipmentMachInsuranceFoldingId",
                table: "EquipmentMachInsuranceFoldingApplicationUsers",
                column: "EquipmentMachInsuranceFoldingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachInsuranceFoldingApplicationUsers");
        }
    }
}
