using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_EntibadosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entibados",
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
                    Performance = table.Column<double>(nullable: false),
                    KS60xMinibox = table.Column<double>(nullable: false),
                    KS100xKMC100 = table.Column<double>(nullable: false),
                    RealzaxExtension = table.Column<double>(nullable: false),
                    Paralelo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entibados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entibados_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entibados_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entibados_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entibados_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entibados_BudgetTitleId",
                table: "Entibados",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Entibados_ProjectFormulaId",
                table: "Entibados",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entibados_ProjectPhaseId",
                table: "Entibados",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Entibados_WorkFrontId",
                table: "Entibados",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entibados");
        }
    }
}
