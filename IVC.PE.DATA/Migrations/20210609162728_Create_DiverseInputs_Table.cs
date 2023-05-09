using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_DiverseInputs_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiverseInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    SupplyId = table.Column<Guid>(nullable: false),
                    BudgetInputId = table.Column<Guid>(nullable: true),
                    ItemNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Metered = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    Parcial = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiverseInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_BudgetInputs_BudgetInputId",
                        column: x => x.BudgetInputId,
                        principalTable: "BudgetInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiverseInputs_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_BudgetInputId",
                table: "DiverseInputs",
                column: "BudgetInputId");

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_BudgetTitleId",
                table: "DiverseInputs",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_ProjectFormulaId",
                table: "DiverseInputs",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_ProjectPhaseId",
                table: "DiverseInputs",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_SupplyId",
                table: "DiverseInputs",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiverseInputs_WorkFrontId",
                table: "DiverseInputs",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiverseInputs");
        }
    }
}
