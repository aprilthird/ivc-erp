using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateMeteredRestatedByPartidasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeteredsRestatedByPartidas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: false),
                    BudgetTitleId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    ItemNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Metered = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteredsRestatedByPartidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByPartidas_BudgetTitles_BudgetTitleId",
                        column: x => x.BudgetTitleId,
                        principalTable: "BudgetTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByPartidas_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByPartidas_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeteredsRestatedByPartidas_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByPartidas_BudgetTitleId",
                table: "MeteredsRestatedByPartidas",
                column: "BudgetTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByPartidas_SewerGroupId",
                table: "MeteredsRestatedByPartidas",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByPartidas_WorkFrontHeadId",
                table: "MeteredsRestatedByPartidas",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByPartidas_WorkFrontId",
                table: "MeteredsRestatedByPartidas",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeteredsRestatedByPartidas");
        }
    }
}
