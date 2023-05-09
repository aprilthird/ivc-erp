using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_ReEntryForReturnsAndReEntryForReturnItems_Update_FieldRequests_AddFileUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "FieldRequests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReEntryForReturns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentNumber = table.Column<int>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReEntryForReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturns_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturns_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturns_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturns_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReEntryForReturnItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReEntryForReturnId = table.Column<Guid>(nullable: false),
                    GoalBudgetInputId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReEntryForReturnItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturnItems_GoalBudgetInputs_GoalBudgetInputId",
                        column: x => x.GoalBudgetInputId,
                        principalTable: "GoalBudgetInputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturnItems_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReEntryForReturnItems_ReEntryForReturns_ReEntryForReturnId",
                        column: x => x.ReEntryForReturnId,
                        principalTable: "ReEntryForReturns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturnItems_GoalBudgetInputId",
                table: "ReEntryForReturnItems",
                column: "GoalBudgetInputId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturnItems_ProjectPhaseId",
                table: "ReEntryForReturnItems",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturnItems_ReEntryForReturnId",
                table: "ReEntryForReturnItems",
                column: "ReEntryForReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturns_ProjectFormulaId",
                table: "ReEntryForReturns",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturns_SewerGroupId",
                table: "ReEntryForReturns",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturns_SupplyFamilyId",
                table: "ReEntryForReturns",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReEntryForReturns_WorkFrontId",
                table: "ReEntryForReturns",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReEntryForReturnItems");

            migrationBuilder.DropTable(
                name: "ReEntryForReturns");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "FieldRequests");
        }
    }
}
