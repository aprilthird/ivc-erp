using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class MachPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentMachParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTypeId = table.Column<Guid>(nullable: false),
                    EquipmentProviderId = table.Column<Guid>(nullable: false),
                    EquipmentMachId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachParts_EquipmentMachs_EquipmentMachId",
                        column: x => x.EquipmentMachId,
                        principalTable: "EquipmentMachs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachParts_EquipmentMachineryTypeTypes_EquipmentMachineryTypeTypeId",
                        column: x => x.EquipmentMachineryTypeTypeId,
                        principalTable: "EquipmentMachineryTypeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachParts_EquipmentProviders_EquipmentProviderId",
                        column: x => x.EquipmentProviderId,
                        principalTable: "EquipmentProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachParts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachPartFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachPartId = table.Column<Guid>(nullable: false),
                    PartNumber = table.Column<string>(nullable: true),
                    PartDate = table.Column<DateTime>(nullable: false),
                    EquipmentMachineryOperatorId = table.Column<Guid>(nullable: false),
                    Specific = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    WorkArea = table.Column<int>(nullable: false),
                    InitHorometer = table.Column<string>(nullable: true),
                    EndHorometer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPartFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPartFoldings_EquipmentMachParts_EquipmentMachPartId",
                        column: x => x.EquipmentMachPartId,
                        principalTable: "EquipmentMachParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPartFoldings_EquipmentMachineryOperators_EquipmentMachineryOperatorId",
                        column: x => x.EquipmentMachineryOperatorId,
                        principalTable: "EquipmentMachineryOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(nullable: false),
                    EquipmentMachineryTypeTypeActivityId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPlusUltra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra_EquipmentMachPartFoldings_EquipmentMachPartFoldingId",
                        column: x => x.EquipmentMachPartFoldingId,
                        principalTable: "EquipmentMachPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra_EquipmentMachineryTypeTypeActivities_EquipmentMachineryTypeTypeActivityId",
                        column: x => x.EquipmentMachineryTypeTypeActivityId,
                        principalTable: "EquipmentMachineryTypeTypeActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_EquipmentMachPartId",
                table: "EquipmentMachPartFoldings",
                column: "EquipmentMachPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_EquipmentMachineryOperatorId",
                table: "EquipmentMachPartFoldings",
                column: "EquipmentMachineryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachParts_EquipmentMachId",
                table: "EquipmentMachParts",
                column: "EquipmentMachId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachParts_EquipmentMachineryTypeTypeId",
                table: "EquipmentMachParts",
                column: "EquipmentMachineryTypeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachParts_EquipmentProviderId",
                table: "EquipmentMachParts",
                column: "EquipmentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachParts_ProjectId",
                table: "EquipmentMachParts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPlusUltra",
                column: "EquipmentMachineryTypeTypeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_ProjectPhaseId",
                table: "EquipmentMachPlusUltra",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_SewerGroupId",
                table: "EquipmentMachPlusUltra",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra");

            migrationBuilder.DropTable(
                name: "EquipmentMachPartFoldings");

            migrationBuilder.DropTable(
                name: "EquipmentMachParts");
        }
    }
}
