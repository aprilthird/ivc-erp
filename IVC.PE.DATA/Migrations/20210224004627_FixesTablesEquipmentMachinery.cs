using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixesTablesEquipmentMachinery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropTable(
                name: "AssignedEquipmentMachineries");

            migrationBuilder.DropTable(
                name: "EquipmentMachineryAssignedUsers");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySofts",
                column: "WorkFrontHeadId",
                principalTable: "WorkFrontHeads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachinerySofts_WorkFrontHeads_WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachinerySofts_WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.DropColumn(
                name: "WorkFrontHeadId",
                table: "EquipmentMachinerySofts");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EquipmentMachineryAssignedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AssignedEquipmentMachineries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachineryAssignedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachineryTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_AssignedEquipmentMachineries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySofts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentMachineryAssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryAssignedUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_AssignedEquipmentMachineries_ProjectId",
                table: "AssignedEquipmentMachineries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryAssignedUsers_ProjectId",
                table: "EquipmentMachineryAssignedUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachineryAssignedUsers_WorkFrontHeadId",
                table: "EquipmentMachineryAssignedUsers",
                column: "WorkFrontHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryAssignedUserId",
                principalTable: "EquipmentMachineryAssignedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachinerySofts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySofts",
                column: "EquipmentMachineryAssignedUserId",
                principalTable: "EquipmentMachineryAssignedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
