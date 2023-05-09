using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddPayrollWeekSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayrollWeekSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectCalendarWeekId = table.Column<Guid>(nullable: false),
                    TotalAllHours = table.Column<decimal>(nullable: false),
                    PawnTotalHours = table.Column<decimal>(nullable: false),
                    PawnHomeTotalHours = table.Column<decimal>(nullable: false),
                    PawnHomeIVCHours = table.Column<decimal>(nullable: false),
                    PawnHomeSyndicateHours = table.Column<decimal>(nullable: false),
                    PawnHomePopulationHours = table.Column<decimal>(nullable: false),
                    PawnCollaboratorTotalHours = table.Column<decimal>(nullable: false),
                    PawnCollaboratorCollaboratorHours = table.Column<decimal>(nullable: false),
                    PawnCollaboratorSyndicateHours = table.Column<decimal>(nullable: false),
                    PawnCollaboratorPopulationHours = table.Column<decimal>(nullable: false),
                    OfficialTotalHours = table.Column<decimal>(nullable: false),
                    OfficialHomeTotalHours = table.Column<decimal>(nullable: false),
                    OfficialHomeIVCHours = table.Column<decimal>(nullable: false),
                    OfficialHomeSyndicateHours = table.Column<decimal>(nullable: false),
                    OfficialHomePopulationHours = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorTotalHours = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorCollaboratorHours = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorSyndicateHours = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorPopulationHours = table.Column<decimal>(nullable: false),
                    OperatorTotalHours = table.Column<decimal>(nullable: false),
                    OperatorHomeTotalHours = table.Column<decimal>(nullable: false),
                    OperatorHomeIVCHours = table.Column<decimal>(nullable: false),
                    OperatorHomeSyndicateHours = table.Column<decimal>(nullable: false),
                    OperatorHomePopulationHours = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorTotalHours = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorCollaboratorHours = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorSyndicateHours = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorPopulationHours = table.Column<decimal>(nullable: false),
                    TotalAllCosts = table.Column<decimal>(nullable: false),
                    PawnTotalCosts = table.Column<decimal>(nullable: false),
                    PawnHomeTotalCosts = table.Column<decimal>(nullable: false),
                    PawnHomeIVCCosts = table.Column<decimal>(nullable: false),
                    PawnHomeSyndicateCosts = table.Column<decimal>(nullable: false),
                    PawnHomePopulationCosts = table.Column<decimal>(nullable: false),
                    PawnCollaboratorTotalCosts = table.Column<decimal>(nullable: false),
                    PawnCollaboratorCollaboratorCosts = table.Column<decimal>(nullable: false),
                    PawnCollaboratorSyndicateCosts = table.Column<decimal>(nullable: false),
                    PawnCollaboratorPopulationCosts = table.Column<decimal>(nullable: false),
                    OfficialTotalCosts = table.Column<decimal>(nullable: false),
                    OfficialHomeTotalCosts = table.Column<decimal>(nullable: false),
                    OfficialHomeIVCCosts = table.Column<decimal>(nullable: false),
                    OfficialHomeSyndicateCosts = table.Column<decimal>(nullable: false),
                    OfficialHomePopulationCosts = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorTotalCosts = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorCollaboratorCosts = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorSyndicateCosts = table.Column<decimal>(nullable: false),
                    OfficialCollaboratorPopulationCosts = table.Column<decimal>(nullable: false),
                    OperatorTotalCosts = table.Column<decimal>(nullable: false),
                    OperatorHomeTotalCosts = table.Column<decimal>(nullable: false),
                    OperatorHomeIVCCosts = table.Column<decimal>(nullable: false),
                    OperatorHomeSyndicateCosts = table.Column<decimal>(nullable: false),
                    OperatorHomePopulationCosts = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorTotalCosts = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorCollaboratorCosts = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorSyndicateCosts = table.Column<decimal>(nullable: false),
                    OperatorCollaboratorPopulationCosts = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollWeekSummaries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollWeekSummaries");
        }
    }
}
