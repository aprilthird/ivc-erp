using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerManifoldCostPerformanceXSewerGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldCostPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TerrainType = table.Column<int>(nullable: false),
                    HeightMin = table.Column<double>(nullable: false),
                    HeightMax = table.Column<double>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Workforce = table.Column<double>(nullable: false),
                    Equipment = table.Column<double>(nullable: false),
                    Services = table.Column<double>(nullable: false),
                    Materials = table.Column<double>(nullable: false),
                    SecurityFactor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldCostPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldCostPerformances_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerManifoldCostPerformanceSewerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldCostPerformanceId = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    WorkforceEquipment = table.Column<double>(nullable: false),
                    WorkforceEquipmentService = table.Column<double>(nullable: false),
                    SecurityFactor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldCostPerformanceSewerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldCostPerformanceSewerGroups_ProjectCalendarWeeks_ProjectCalendarWeekId",
                        column: x => x.ProjectCalendarWeekId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldCostPerformanceSewerGroups_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldCostPerformanceSewerGroups_SewerManifoldCostPerformances_SewerManifoldCostPerformanceId",
                        column: x => x.SewerManifoldCostPerformanceId,
                        principalTable: "SewerManifoldCostPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldCostPerformances_ProjectId",
                table: "SewerManifoldCostPerformances",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldCostPerformanceSewerGroups_ProjectCalendarWeekId",
                table: "SewerManifoldCostPerformanceSewerGroups",
                column: "ProjectCalendarWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldCostPerformanceSewerGroups_SewerGroupId",
                table: "SewerManifoldCostPerformanceSewerGroups",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldCostPerformanceSewerGroups_SewerManifoldCostPerformanceId",
                table: "SewerManifoldCostPerformanceSewerGroups",
                column: "SewerManifoldCostPerformanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldCostPerformanceSewerGroups");

            migrationBuilder.DropTable(
                name: "SewerManifoldCostPerformances");
        }
    }
}
