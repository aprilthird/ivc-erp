using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class EquipmentMachinerySoftPartPlusUltra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    PartDate = table.Column<DateTime>(nullable: false),
                    EquipmentMachineryTypeSoftId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryAssignedUserId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUsers_EquipmentMachineryAssignedUserId",
                        column: x => x.EquipmentMachineryAssignedUserId,
                        principalTable: "EquipmentMachineryAssignedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_EquipmentMachinerySofts_EquipmentMachinerySoftId",
                        column: x => x.EquipmentMachinerySoftId,
                        principalTable: "EquipmentMachinerySofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_EquipmentMachineryTypeSofts_EquipmentMachineryTypeSoftId",
                        column: x => x.EquipmentMachineryTypeSoftId,
                        principalTable: "EquipmentMachineryTypeSofts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftParts_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachinerySoftPartPlusUltra",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachinerySoftPartId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeSoftActivityId = table.Column<Guid>(nullable: false),
                    Specific = table.Column<string>(nullable: true),
                    StartMileage = table.Column<double>(nullable: false),
                    EndMileage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachinerySoftPartPlusUltra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftParts_EquipmentMachinerySoftPartId",
                        column: x => x.EquipmentMachinerySoftPartId,
                        principalTable: "EquipmentMachinerySoftParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachinerySoftPartPlusUltra_EquipmentMachineryTypeSoftActivites_EquipmentMachineryTypeSoftActivityId",
                        column: x => x.EquipmentMachineryTypeSoftActivityId,
                        principalTable: "EquipmentMachineryTypeSoftActivites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachinerySoftPartId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachinerySoftPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftPartPlusUltra_EquipmentMachineryTypeSoftActivityId",
                table: "EquipmentMachinerySoftPartPlusUltra",
                column: "EquipmentMachineryTypeSoftActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryAssignedUserId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryAssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryOperatorId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachinerySoftId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachinerySoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentMachineryTypeSoftId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentMachineryTypeSoftId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_EquipmentProviderId",
                table: "EquipmentMachinerySoftParts",
                column: "EquipmentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_ProjectId",
                table: "EquipmentMachinerySoftParts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachinerySoftParts_ProjectPhaseId",
                table: "EquipmentMachinerySoftParts",
                column: "ProjectPhaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftPartPlusUltra");

            migrationBuilder.DropTable(
                name: "EquipmentMachinerySoftParts");
        }
    }
}
