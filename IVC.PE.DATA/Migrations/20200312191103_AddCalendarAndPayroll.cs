using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddCalendarAndPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayrollVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Formula = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollVariables", x => x.Id);
                });

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
                name: "WorkerDailyTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    SystemPhaseId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
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
                        name: "FK_WorkerDailyTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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

            migrationBuilder.CreateTable(
                name: "PayrollConcepts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    PayrollVariableId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollConcepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollConcepts_PayrollVariables_PayrollVariableId",
                        column: x => x.PayrollVariableId,
                        principalTable: "PayrollVariables",
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
                name: "PayrollConceptFormulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PayrollConceptoId = table.Column<Guid>(nullable: false),
                    PayrollConceptId = table.Column<Guid>(nullable: true),
                    LaborRegimeId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Formula = table.Column<string>(nullable: true),
                    IsAffectedToEsSalud = table.Column<bool>(nullable: false),
                    IsAffectedToOnp = table.Column<bool>(nullable: false),
                    IsAffectedToQta = table.Column<bool>(nullable: false),
                    IsAffectedToAfp = table.Column<bool>(nullable: false),
                    IsAffectedToRetJud = table.Column<bool>(nullable: false),
                    IsComputableToCTS = table.Column<bool>(nullable: false),
                    IsComputableToGrati = table.Column<bool>(nullable: false),
                    IsComputableToVacac = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollConceptFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollConceptFormulas_PayrollConcepts_PayrollConceptId",
                        column: x => x.PayrollConceptId,
                        principalTable: "PayrollConcepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollMovementHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ProjectCalendarId = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: true),
                    ProjectCalendarMonthID = table.Column<Guid>(nullable: true),
                    UIT = table.Column<decimal>(nullable: false),
                    MinimumWage = table.Column<decimal>(nullable: false),
                    DollarExchangeRate = table.Column<decimal>(nullable: false),
                    MaximumInsurableRemuneration = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollMovementHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollMovementHeaders_ProjectCalendars_ProjectCalendarId",
                        column: x => x.ProjectCalendarId,
                        principalTable: "ProjectCalendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementHeaders_ProjectCalendarMonths_ProjectCalendarMonthID",
                        column: x => x.ProjectCalendarMonthID,
                        principalTable: "ProjectCalendarMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementHeaders_ProjectCalendarWeeks_ProjectCalendarWeekId",
                        column: x => x.ProjectCalendarWeekId,
                        principalTable: "ProjectCalendarWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementHeaders_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollMovementDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PayrollMovementHeaderId = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    PayrollVariableId = table.Column<Guid>(nullable: true),
                    PayrollConceptId = table.Column<Guid>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollMovementDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollMovementDetails_PayrollConcepts_PayrollConceptId",
                        column: x => x.PayrollConceptId,
                        principalTable: "PayrollConcepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementDetails_PayrollMovementHeaders_PayrollMovementHeaderId",
                        column: x => x.PayrollMovementHeaderId,
                        principalTable: "PayrollMovementHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementDetails_PayrollVariables_PayrollVariableId",
                        column: x => x.PayrollVariableId,
                        principalTable: "PayrollVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollMovementDetails_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollPensionFundAdministratorRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PensionFundAdministratorId = table.Column<Guid>(nullable: false),
                    PayrollMovementHeaderId = table.Column<Guid>(nullable: false),
                    FundRate = table.Column<decimal>(nullable: false),
                    FlowComissionRate = table.Column<decimal>(nullable: false),
                    MixedComissionRate = table.Column<decimal>(nullable: false),
                    EarlyRetirementRate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollPensionFundAdministratorRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollPensionFundAdministratorRates_PayrollMovementHeaders_PayrollMovementHeaderId",
                        column: x => x.PayrollMovementHeaderId,
                        principalTable: "PayrollMovementHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollPensionFundAdministratorRates_PensionFundAdministrators_PensionFundAdministratorId",
                        column: x => x.PensionFundAdministratorId,
                        principalTable: "PensionFundAdministrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollWorkerCategoryWages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PayrollMovementHeaderId = table.Column<Guid>(nullable: false),
                    WorkerCategoryId = table.Column<int>(nullable: false),
                    DayWage = table.Column<decimal>(nullable: false),
                    BUCRate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollWorkerCategoryWages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollWorkerCategoryWages_PayrollMovementHeaders_PayrollMovementHeaderId",
                        column: x => x.PayrollMovementHeaderId,
                        principalTable: "PayrollMovementHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollConceptFormulas_PayrollConceptId",
                table: "PayrollConceptFormulas",
                column: "PayrollConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollConcepts_PayrollVariableId",
                table: "PayrollConcepts",
                column: "PayrollVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_PayrollConceptId",
                table: "PayrollMovementDetails",
                column: "PayrollConceptId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_PayrollMovementHeaderId",
                table: "PayrollMovementDetails",
                column: "PayrollMovementHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_PayrollVariableId",
                table: "PayrollMovementDetails",
                column: "PayrollVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementDetails_WorkerId",
                table: "PayrollMovementDetails",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarMonthID",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarMonthID");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectCalendarWeekId",
                table: "PayrollMovementHeaders",
                column: "ProjectCalendarWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollMovementHeaders_ProjectId",
                table: "PayrollMovementHeaders",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPensionFundAdministratorRates_PayrollMovementHeaderId",
                table: "PayrollPensionFundAdministratorRates",
                column: "PayrollMovementHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPensionFundAdministratorRates_PensionFundAdministratorId",
                table: "PayrollPensionFundAdministratorRates",
                column: "PensionFundAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollWorkerCategoryWages_PayrollMovementHeaderId",
                table: "PayrollWorkerCategoryWages",
                column: "PayrollMovementHeaderId");

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
                name: "IX_WorkerDailyTasks_ProjectId",
                table: "WorkerDailyTasks",
                column: "ProjectId");

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
                name: "PayrollConceptFormulas");

            migrationBuilder.DropTable(
                name: "PayrollMovementDetails");

            migrationBuilder.DropTable(
                name: "PayrollPensionFundAdministratorRates");

            migrationBuilder.DropTable(
                name: "PayrollWorkerCategoryWages");

            migrationBuilder.DropTable(
                name: "WorkerDailyTasks");

            migrationBuilder.DropTable(
                name: "PayrollConcepts");

            migrationBuilder.DropTable(
                name: "PayrollMovementHeaders");

            migrationBuilder.DropTable(
                name: "PayrollVariables");

            migrationBuilder.DropTable(
                name: "ProjectCalendarMonths");

            migrationBuilder.DropTable(
                name: "ProjectCalendarWeeks");

            migrationBuilder.DropTable(
                name: "ProjectCalendars");
        }
    }
}
