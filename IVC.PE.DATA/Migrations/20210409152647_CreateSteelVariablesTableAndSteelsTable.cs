using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateSteelVariablesTableAndSteelsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Steels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    ItemNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Steels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steels_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Steels_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SteelVariables",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RodDiameterInch = table.Column<string>(nullable: true),
                    RodDiameterMilimeters = table.Column<double>(nullable: false),
                    Section = table.Column<int>(nullable: false),
                    Perimeter = table.Column<double>(nullable: false),
                    NominalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteelVariables", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Steels_BudgetTitleId",
                table: "Steels",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Steels_ProjectFormulaId",
                table: "Steels",
                column: "ProjectFormulaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Steels");

            migrationBuilder.DropTable(
                name: "SteelVariables");
        }
    }
}
