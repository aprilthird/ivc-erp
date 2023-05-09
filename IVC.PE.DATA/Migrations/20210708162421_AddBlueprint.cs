using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBlueprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blueprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    SpecialityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    BlueprintDate = table.Column<DateTime>(nullable: false),
                    TechnicalVersionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blueprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blueprints_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blueprints_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blueprints_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blueprints_TechnicalVersions_TechnicalVersionId",
                        column: x => x.TechnicalVersionId,
                        principalTable: "TechnicalVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_BudgetTitleId",
                table: "Blueprints",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_ProjectFormulaId",
                table: "Blueprints",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_SpecialityId",
                table: "Blueprints",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_TechnicalVersionId",
                table: "Blueprints",
                column: "TechnicalVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blueprints");
        }
    }
}
