using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateOCBudgetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OCBudgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    NumberItem = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    BudgetGroupId = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    BudgetFormulaId = table.Column<Guid>(nullable: false),
                    BudgetTypeId = table.Column<Guid>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Metered = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ContractualMO = table.Column<double>(nullable: false),
                    ContractualEQ = table.Column<double>(nullable: false),
                    ContractualSubcontract = table.Column<double>(nullable: false),
                    ContractualMaterials = table.Column<double>(nullable: false),
                    CollaboratorMO = table.Column<double>(nullable: false),
                    CollaboratorEQ = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OCBudgets_BudgetFormulas_BudgetFormulaId",
                        column: x => x.BudgetFormulaId,
                        principalTable: "BudgetFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCBudgets_BudgetGroups_BudgetGroupId",
                        column: x => x.BudgetGroupId,
                        principalTable: "BudgetGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCBudgets_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OCBudgets_BudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalTable: "BudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_BudgetFormulaId",
                table: "OCBudgets",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_BudgetGroupId",
                table: "OCBudgets",
                column: "BudgetGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_BudgetTitleId",
                table: "OCBudgets",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_OCBudgets_BudgetTypeId",
                table: "OCBudgets",
                column: "BudgetTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OCBudgets");
        }
    }
}
