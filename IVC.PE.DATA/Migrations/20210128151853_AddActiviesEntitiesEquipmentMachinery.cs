using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddActiviesEntitiesEquipmentMachinery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTypeSoftActivites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTypeSoftActivites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTypeSoftActivites_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryTypeTypeActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryTypeTypeActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryTypeTypeActivities_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTypeSoftActivites_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachineryTypeSoftActivites",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryTypeTypeActivities_EquipmentMachineryTypeTypeId",
                table: "EquipmentMachineryTypeTypeActivities",
                column: "EquipmentMachineryTypeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryTypeSoftActivites");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryTypeTypeActivities");
        }
    }
}
