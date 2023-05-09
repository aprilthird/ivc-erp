using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_ConsolidatedSteels_Table_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedSteels",
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
                    ContractualMetered = table.Column<double>(nullable: false),
                    Rod6mm = table.Column<double>(nullable: false),
                    Rod3x8 = table.Column<double>(nullable: false),
                    Rod1x2 = table.Column<double>(nullable: false),
                    Rod5x8 = table.Column<double>(nullable: false),
                    Rod3x4 = table.Column<double>(nullable: false),
                    Rod1 = table.Column<double>(nullable: false),
                    ContractualStaked = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidatedSteels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsolidatedSteels_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedSteels_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedSteels_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedSteels_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedSteels_BudgetTitleId",
                table: "ConsolidatedSteels",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedSteels_ProjectFormulaId",
                table: "ConsolidatedSteels",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedSteels_ProjectPhaseId",
                table: "ConsolidatedSteels",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedSteels_WorkFrontId",
                table: "ConsolidatedSteels",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedSteels");
        }
    }
}
