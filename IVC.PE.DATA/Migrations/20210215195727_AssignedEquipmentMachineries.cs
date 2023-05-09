using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AssignedEquipmentMachineries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignedEquipmentMachineries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachineryAssignedUserId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: true),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedEquipmentMachineries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedEquipmentMachineries_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                        column: x => x.EquipmentMachineryAssignedUserId,
                        principalTable: "EquipmentMachineryAssignedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignedEquipmentMachineries_EquipmentMachineryTypes_EquipmentMachineryTypeId",
                        column: x => x.EquipmentMachineryTypeId,
                        principalTable: "EquipmentMachineryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignedEquipmentMachineries_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignedEquipmentMachineries_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_EquipmentMachineryAssignedUserId",
                table: "AssignedEquipmentMachineries",
                column: "EquipmentMachineryAssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_EquipmentMachineryTypeId",
                table: "AssignedEquipmentMachineries",
                column: "EquipmentMachineryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_EquipmentMachineryTypeSoftId",
                table: "AssignedEquipmentMachineries",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_EquipmentMachineryTypeTypeId",
                table: "AssignedEquipmentMachineries",
                column: "EquipmentMachineryTypeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedEquipmentMachineries");
        }
    }
}
