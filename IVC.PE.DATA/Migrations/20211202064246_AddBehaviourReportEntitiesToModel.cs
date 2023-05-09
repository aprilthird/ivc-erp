using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBehaviourReportEntitiesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BehaviourReportCauses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CauseDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourReportCauses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BehaviourReportItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BehavoiurDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourReportItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BehaviourReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ObsLessThan3Months = table.Column<bool>(nullable: false),
                    DayTurn = table.Column<bool>(nullable: false),
                    ObsDate = table.Column<DateTime>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaviourReports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BehaviourReports_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BehaviourReportSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    BehviourReportCode = table.Column<string>(nullable: true),
                    BehviourReportCount = table.Column<int>(nullable: false),
                    VersionCode = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    VersionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourReportSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaviourReportSummaries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BehaviourReportDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BehaviourReportId = table.Column<Guid>(nullable: false),
                    BehaviourReportItemId = table.Column<Guid>(nullable: false),
                    BehaviourReportCauseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviourReportDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaviourReportDetails_BehaviourReportCauses_BehaviourReportCauseId",
                        column: x => x.BehaviourReportCauseId,
                        principalTable: "BehaviourReportCauses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BehaviourReportDetails_BehaviourReports_BehaviourReportId",
                        column: x => x.BehaviourReportId,
                        principalTable: "BehaviourReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BehaviourReportDetails_BehaviourReportItems_BehaviourReportItemId",
                        column: x => x.BehaviourReportItemId,
                        principalTable: "BehaviourReportItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReportDetails_BehaviourReportCauseId",
                table: "BehaviourReportDetails",
                column: "BehaviourReportCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReportDetails_BehaviourReportId",
                table: "BehaviourReportDetails",
                column: "BehaviourReportId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReportDetails_BehaviourReportItemId",
                table: "BehaviourReportDetails",
                column: "BehaviourReportItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReports_ProjectId",
                table: "BehaviourReports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReports_SewerGroupId",
                table: "BehaviourReports",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviourReportSummaries_ProjectId",
                table: "BehaviourReportSummaries",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehaviourReportDetails");

            migrationBuilder.DropTable(
                name: "BehaviourReportSummaries");

            migrationBuilder.DropTable(
                name: "BehaviourReportCauses");

            migrationBuilder.DropTable(
                name: "BehaviourReports");

            migrationBuilder.DropTable(
                name: "BehaviourReportItems");
        }
    }
}
