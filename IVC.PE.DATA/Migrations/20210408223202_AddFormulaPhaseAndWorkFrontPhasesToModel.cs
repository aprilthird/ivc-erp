using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddFormulaPhaseAndWorkFrontPhasesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFormulaPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFormulaPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaPhases_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFrontProjectPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    PorjectPhaseId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFrontProjectPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFrontProjectPhases_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFrontProjectPhases_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaPhases_ProjectFormulaId",
                table: "ProjectFormulaPhases",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaPhases_ProjectPhaseId",
                table: "ProjectFormulaPhases",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontProjectPhases_ProjectPhaseId",
                table: "WorkFrontProjectPhases",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontProjectPhases_WorkFrontId",
                table: "WorkFrontProjectPhases",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFormulaPhases");

            migrationBuilder.DropTable(
                name: "WorkFrontProjectPhases");
        }
    }
}
