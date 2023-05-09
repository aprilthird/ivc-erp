using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_TechosTable_Update_OrdersTable_AddPdfFileUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfFileUrl",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Techos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MeasurementUnitId = table.Column<Guid>(nullable: false),
                    Metered = table.Column<double>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Techos");

            migrationBuilder.DropColumn(
                name: "PdfFileUrl",
                table: "Orders");
        }
    }
}
