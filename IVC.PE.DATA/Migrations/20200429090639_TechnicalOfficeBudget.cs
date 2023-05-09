using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class TechnicalOfficeBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDirectCost",
                table: "BudgetFormulas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BudgetInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    MeasurementUnitId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false),
                    SaleUnitPrice = table.Column<double>(nullable: true),
                    GoalUnitPrice = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetInputs_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputs_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputs_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetTitles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetType = table.Column<int>(nullable: false),
                    BudgetGroup = table.Column<int>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    BudgetFormulaId = table.Column<Guid>(nullable: false),
                    SaleValue = table.Column<double>(nullable: true),
                    GoalValue = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_BudgetFormulas_BudgetFormulaId",
                        column: x => x.BudgetFormulaId,
                        principalTable: "BudgetFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetInputAllocations",
                columns: table => new
                {
                    BudgetInputId = table.Column<Guid>(nullable: false),
                    BudgetId = table.Column<Guid>(nullable: false),
                    Measure = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetInputAllocations", x => new { x.BudgetId, x.BudgetInputId });
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocations_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocations_BudgetInputs_BudgetInputId",
                        column: x => x.BudgetInputId,
                        principalTable: "BudgetInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetInputAllocationGroups",
                columns: table => new
                {
                    BudgetInputId = table.Column<Guid>(nullable: false),
                    BudgetId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    Measure = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetInputAllocationGroups", x => new { x.BudgetId, x.BudgetInputId, x.SewerGroupId });
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocationGroups_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocationGroups_BudgetInputs_BudgetInputId",
                        column: x => x.BudgetInputId,
                        principalTable: "BudgetInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocationGroups_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetInputAllocationGroups_BudgetInputAllocations_BudgetId_BudgetInputId",
                        columns: x => new { x.BudgetId, x.BudgetInputId },
                        principalTable: "BudgetInputAllocations",
                        principalColumns: new[] { "BudgetId", "BudgetInputId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocationGroups_BudgetInputId",
                table: "BudgetInputAllocationGroups",
                column: "BudgetInputId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocationGroups_SewerGroupId",
                table: "BudgetInputAllocationGroups",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputAllocations_BudgetInputId",
                table: "BudgetInputAllocations",
                column: "BudgetInputId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_MeasurementUnitId",
                table: "BudgetInputs",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_ProjectId",
                table: "BudgetInputs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_SupplyFamilyId",
                table: "BudgetInputs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetInputs_SupplyGroupId",
                table: "BudgetInputs",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetFormulaId",
                table: "Budgets",
                column: "BudgetFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTitleId",
                table: "Budgets",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTitles_ProjectId",
                table: "BudgetTitles",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetInputAllocationGroups");

            migrationBuilder.DropTable(
                name: "BudgetInputAllocations");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "BudgetInputs");

            migrationBuilder.DropTable(
                name: "BudgetTitles");

            migrationBuilder.DropColumn(
                name: "IsDirectCost",
                table: "BudgetFormulas");
        }
    }
}
