using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateExpensesUtilityTable_And_UpdateProjectFormulaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "ProjectFormulas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExpensesUtilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Group = table.Column<int>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    BudgetFormulaId = table.Column<Guid>(nullable: false),
                    BudgetTypeId = table.Column<Guid>(nullable: true),
                    FixedGeneralExpense = table.Column<double>(nullable: false),
                    FixedGeneralPercentage = table.Column<double>(nullable: false),
                    VariableGeneralExpense = table.Column<double>(nullable: false),
                    VariableGeneralPercentage = table.Column<double>(nullable: false),
                    Utility = table.Column<double>(nullable: false),
                    UtilityPercentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesUtilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensesUtilities_BudgetFormulas_BudgetFormulaId",
                        column: x => x.BudgetFormulaId,
                        principalTable: "BudgetFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensesUtilities_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensesUtilities_BudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalTable: "BudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesUtilities_BudgetFormulaId",
                table: "ExpensesUtilities",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesUtilities_BudgetTitleId",
                table: "ExpensesUtilities",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesUtilities_BudgetTypeId",
                table: "ExpensesUtilities",
                column: "BudgetTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensesUtilities");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "ProjectFormulas");
        }
    }
}
