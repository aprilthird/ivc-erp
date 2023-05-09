using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SupplyEntriesTable_AddRemissionGuideName_Create_FieldRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RemissionGuide",
                table: "SupplyEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FieldRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Observation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldRequests_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequests_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequests_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequests_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequests_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequests_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_BudgetTitleId",
                table: "FieldRequests",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_ProjectFormulaId",
                table: "FieldRequests",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_SewerGroupId",
                table: "FieldRequests",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_SupplyFamilyId",
                table: "FieldRequests",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_WarehouseId",
                table: "FieldRequests",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequests_WorkFrontId",
                table: "FieldRequests",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldRequests");

            migrationBuilder.DropColumn(
                name: "RemissionGuide",
                table: "SupplyEntries");
        }
    }
}
