using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateGoalBudgetInputsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalBudgetInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    MeasurementUnitId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: true),
                    SaleUnitPrice = table.Column<double>(nullable: false),
                    GoalUnitPrice = table.Column<double>(nullable: false),
                    Group = table.Column<int>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: true),
                    ProjectFormulaId = table.Column<Guid>(nullable: true),
                    BudgetFormulaId = table.Column<Guid>(nullable: true),
                    BudgetTypeId = table.Column<Guid>(nullable: true),
                    Metered = table.Column<double>(nullable: false),
                    Parcial = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalBudgetInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_BudgetFormulas_BudgetFormulaId",
                        column: x => x.BudgetFormulaId,
                        principalTable: "BudgetFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_BudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalTable: "BudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalBudgetInputs_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_BudgetFormulaId",
                table: "GoalBudgetInputs",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_BudgetTitleId",
                table: "GoalBudgetInputs",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_BudgetTypeId",
                table: "GoalBudgetInputs",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_MeasurementUnitId",
                table: "GoalBudgetInputs",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_ProjectFormulaId",
                table: "GoalBudgetInputs",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_ProjectId",
                table: "GoalBudgetInputs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_SupplyFamilyId",
                table: "GoalBudgetInputs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalBudgetInputs_SupplyGroupId",
                table: "GoalBudgetInputs",
                column: "SupplyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalBudgetInputs");
        }
    }
}
