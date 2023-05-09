using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemovePlus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra");

            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra2");

            migrationBuilder.DropTable(
                name: "EquipmentMachPlusUltra3");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "EquipmentMachPartFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings",
                column: "EquipmentMachineryTypeTypeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPartFoldings_SewerGroupId",
                table: "EquipmentMachPartFoldings",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_EquipmentMachineryTypeTypeActivities_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings",
                column: "EquipmentMachineryTypeTypeActivityId",
                principalTable: "EquipmentMachineryTypeTypeActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPartFoldings",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentMachPartFoldings_SewerGroups_SewerGroupId",
                table: "EquipmentMachPartFoldings",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_EquipmentMachineryTypeTypeActivities_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_ProjectPhases_ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentMachPartFoldings_SewerGroups_SewerGroupId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentMachPartFoldings_SewerGroupId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "EquipmentMachPartFoldings");

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachineryTypeTypeActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SewerGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPlusUltra2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra2_EquipmentMachPartFoldings_EquipmentMachPartFoldingId",
                        column: x => x.EquipmentMachPartFoldingId,
                        principalTable: "EquipmentMachPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra2_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMachPlusUltra3",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentMachPartFoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMachPlusUltra3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra3_EquipmentMachPartFoldings_EquipmentMachPartFoldingId",
                        column: x => x.EquipmentMachPartFoldingId,
                        principalTable: "EquipmentMachPartFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMachPlusUltra3_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra_EquipmentMachineryTypeTypeActivityId",
                table: "EquipmentMachPlusUltra",
                column: "EquipmentMachineryTypeTypeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra2_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra2",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra2_SewerGroupId",
                table: "EquipmentMachPlusUltra2",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra3_EquipmentMachPartFoldingId",
                table: "EquipmentMachPlusUltra3",
                column: "EquipmentMachPartFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMachPlusUltra3_ProjectPhaseId",
                table: "EquipmentMachPlusUltra3",
                column: "ProjectPhaseId");
        }
    }
}
