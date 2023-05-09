using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddEquipmentMachineryAssignedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachineryAssignedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachineryAssignedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryAssignedUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachineryAssignedUsers_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryAssignedUsers_ProjectId",
                table: "EquipmentMachineryAssignedUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryAssignedUsers_WorkFrontHeadId",
                table: "EquipmentMachineryAssignedUsers",
                column: "WorkFrontHeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachineryAssignedUsers");
        }
    }
}
