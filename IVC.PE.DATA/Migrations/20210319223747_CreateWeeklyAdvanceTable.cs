using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateWeeklyAdvanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyAdvances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    WorkersNumberOP = table.Column<int>(nullable: false),
                    WorkersNumberOF = table.Column<int>(nullable: false),
                    WorkersNumberPE = table.Column<int>(nullable: false),
                    WorkerNumberTotal = table.Column<int>(nullable: false),
                    SaleMO = table.Column<double>(nullable: false),
                    SaleEQ = table.Column<double>(nullable: false),
                    SaleTotal = table.Column<double>(nullable: false),
                    GoalMO = table.Column<double>(nullable: false),
                    GoalEQ = table.Column<double>(nullable: false),
                    GoalTotal = table.Column<double>(nullable: false),
                    CostMO = table.Column<double>(nullable: false),
                    CostEQ = table.Column<double>(nullable: false),
                    CostTotal = table.Column<double>(nullable: false),
                    MarginActual = table.Column<double>(nullable: false),
                    MarginAccumulated = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyAdvances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyAdvances_ProjectCalendarWeeks_ProjectCalendarWeekId",
                        column: x => x.ProjectCalendarWeekId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyAdvances_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyAdvances_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyAdvances_ProjectCalendarWeekId",
                table: "WeeklyAdvances",
                column: "ProjectCalendarWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyAdvances_SewerGroupId",
                table: "WeeklyAdvances",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyAdvances_WorkFrontHeadId",
                table: "WeeklyAdvances",
                column: "WorkFrontHeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyAdvances");
        }
    }
}
