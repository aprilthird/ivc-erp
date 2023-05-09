using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DeleteConsolidatedSteelsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedSteels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedSteels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractualMetered = table.Column<double>(type: "float", nullable: false),
                    ContractualStaked = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metered = table.Column<double>(type: "float", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    ProjectFormulaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rod1 = table.Column<double>(type: "float", nullable: false),
                    Rod1x2 = table.Column<double>(type: "float", nullable: false),
                    Rod3x4 = table.Column<double>(type: "float", nullable: false),
                    Rod3x8 = table.Column<double>(type: "float", nullable: false),
                    Rod5x8 = table.Column<double>(type: "float", nullable: false),
                    Rod6mm = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkFrontId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
    }
}
