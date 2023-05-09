using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_ConsolidatedAmountEntibados_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedAmountEntibados",
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
                    Performance = table.Column<double>(nullable: false),
                    KS60xMinibox = table.Column<double>(nullable: false),
                    KS100xKMC100 = table.Column<double>(nullable: false),
                    RealzaxExtension = table.Column<double>(nullable: false),
                    Corredera = table.Column<double>(nullable: false),
                    Paralelo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidatedAmountEntibados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsolidatedAmountEntibados_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedAmountEntibados_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedAmountEntibados_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedAmountEntibados_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedAmountEntibados_BudgetTitleId",
                table: "ConsolidatedAmountEntibados",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedAmountEntibados_ProjectFormulaId",
                table: "ConsolidatedAmountEntibados",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedAmountEntibados_ProjectPhaseId",
                table: "ConsolidatedAmountEntibados",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedAmountEntibados_WorkFrontId",
                table: "ConsolidatedAmountEntibados",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedAmountEntibados");
        }
    }
}
