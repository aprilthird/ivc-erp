using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddToDbContextMeteredsRestatedByStreetchs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeteredsRestatedByStreetchs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BudgetTittleId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    ItemNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Metered = table.Column<string>(nullable: true),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteredsRestatedByStreetchs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByStreetchs_BudgetTitles_BudgetTittleId",
                        column: x => x.BudgetTittleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByStreetchs_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByStreetchs_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByStreetchs_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_BudgetTittleId",
                table: "MeteredsRestatedByStreetchs",
                column: "BudgetTittleId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_ProjectFormulaId",
                table: "MeteredsRestatedByStreetchs",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_SewerGroupId",
                table: "MeteredsRestatedByStreetchs",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_WorkFrontId",
                table: "MeteredsRestatedByStreetchs",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeteredsRestatedByStreetchs");
        }
    }
}
