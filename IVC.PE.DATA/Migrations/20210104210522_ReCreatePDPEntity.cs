using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ReCreatePDPEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionDailyParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectFormula = table.Column<int>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionDailyParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionDailyParts_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionDailyParts_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionDailyParts_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionDailyParts_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_SewerGroupId",
                table: "ProductionDailyParts",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_SewerManifoldId",
                table: "ProductionDailyParts",
                column: "SewerManifoldId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_WorkFrontHeadId",
                table: "ProductionDailyParts",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDailyParts_WorkFrontId",
                table: "ProductionDailyParts",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionDailyParts");
        }
    }
}
