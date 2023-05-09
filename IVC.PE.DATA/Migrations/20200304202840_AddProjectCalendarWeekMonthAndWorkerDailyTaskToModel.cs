using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectCalendarWeekMonthAndWorkerDailyTaskToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCalendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    IsWeekly = table.Column<bool>(nullable: false),
                    FirstDayOfThCalendar = table.Column<DateTime>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCalendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCalendars_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCalendarMonths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectCalendarId = table.Column<Guid>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    YearMonthNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProcessType = table.Column<int>(nullable: false),
                    MonthStart = table.Column<DateTime>(nullable: false),
                    MonthEnd = table.Column<DateTime>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCalendarMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCalendarMonths_ProjectCalendars_ProjectCalendarId",
                        column: x => x.ProjectCalendarId,
                        principalTable: "ProjectCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCalendarWeeks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectCalendarId = table.Column<Guid>(nullable: false),
                    WeekNumber = table.Column<int>(nullable: false),
                    YearWeekNumber = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProcessType = table.Column<int>(nullable: false),
                    WeekStart = table.Column<DateTime>(nullable: false),
                    WeekEnd = table.Column<DateTime>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCalendarWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCalendarWeeks_ProjectCalendars_ProjectCalendarId",
                        column: x => x.ProjectCalendarId,
                        principalTable: "ProjectCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkerDailyTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    SystemPhaseId = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeeklyId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    HoursNormal = table.Column<decimal>(nullable: false),
                    Hours60Percent = table.Column<decimal>(nullable: false),
                    Hours100Percent = table.Column<decimal>(nullable: false),
                    HoursMedicalRest = table.Column<decimal>(nullable: false),
                    HoursMedicalLeave = table.Column<decimal>(nullable: false),
                    HoursPaternityLeave = table.Column<decimal>(nullable: false),
                    HoursHoliday = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerDailyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerDailyTasks_ProjectCalendarWeeks_ProjectCalendarWeeklyId",
                        column: x => x.ProjectCalendarWeeklyId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerDailyTasks_SystemPhases_SystemPhaseId",
                        column: x => x.SystemPhaseId,
                        principalTable: "SystemPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerDailyTasks_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCalendarMonths_ProjectCalendarId",
                table: "ProjectCalendarMonths",
                column: "ProjectCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCalendars_ProjectId",
                table: "ProjectCalendars",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCalendarWeeks_ProjectCalendarId",
                table: "ProjectCalendarWeeks",
                column: "ProjectCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerDailyTasks_ProjectCalendarWeeklyId",
                table: "WorkerDailyTasks",
                column: "ProjectCalendarWeeklyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerDailyTasks_SystemPhaseId",
                table: "WorkerDailyTasks",
                column: "SystemPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerDailyTasks_WorkerId",
                table: "WorkerDailyTasks",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCalendarMonths");

            migrationBuilder.DropTable(
                name: "WorkerDailyTasks");

            migrationBuilder.DropTable(
                name: "ProjectCalendarWeeks");

            migrationBuilder.DropTable(
                name: "ProjectCalendars");
        }
    }
}
