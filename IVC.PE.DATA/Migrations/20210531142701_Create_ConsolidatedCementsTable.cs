using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_ConsolidatedCementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedCements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    ItemNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Metered = table.Column<double>(nullable: false),
                    ContractualMeteredTypeOne = table.Column<double>(nullable: false),
                    ContractualMeteredTypeFive = table.Column<double>(nullable: false),
                    ContractualRestatedTypeOne = table.Column<double>(nullable: false),
                    ContractualRestatedTypeFive = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidatedCements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsolidatedCements_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedCements_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedCements_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedCements_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedCements_BudgetTitleId",
                table: "ConsolidatedCements",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedCements_ProjectFormulaId",
                table: "ConsolidatedCements",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedCements_ProjectPhaseId",
                table: "ConsolidatedCements",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedCements_WorkFrontId",
                table: "ConsolidatedCements",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedCements");
        }
    }
}
