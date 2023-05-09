using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class DeleteTechosTable_CreateSupplyEntriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Techos");

            migrationBuilder.CreateTable(
                name: "SuppliyEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentNumber = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    RemissionGuide = table.Column<string>(nullable: true),
                    ProviderId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliyEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppliyEntries_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuppliyEntries_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuppliyEntries_OrderId",
                table: "SuppliyEntries",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliyEntries_ProviderId",
                table: "SuppliyEntries",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuppliyEntries");

            migrationBuilder.CreateTable(
                name: "Techos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Metered = table.Column<double>(type: "float", nullable: false),
                    ProjectFormulaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyFamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplyGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFrontId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Techos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Techos_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Techos_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Techos_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Techos_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Techos_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Techos_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Techos_BudgetTitleId",
                table: "Techos",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Techos_MeasurementUnitId",
                table: "Techos",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Techos_ProjectFormulaId",
                table: "Techos",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Techos_SupplyFamilyId",
                table: "Techos",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Techos_SupplyGroupId",
                table: "Techos",
                column: "SupplyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Techos_WorkFrontId",
                table: "Techos",
                column: "WorkFrontId");
        }
    }
}
