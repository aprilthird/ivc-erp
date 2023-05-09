using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddAggregationRequestToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestNumber = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    AggregationStockId = table.Column<Guid>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Turn = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregationRequests_AggregationStocks_AggregationStockId",
                        column: x => x.AggregationStockId,
                        principalTable: "AggregationStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationRequests_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationRequests_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationRequests_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AggregationRequests_AggregationStockId",
                table: "AggregationRequests",
                column: "AggregationStockId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationRequests_ProjectFormulaId",
                table: "AggregationRequests",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationRequests_ProjectId",
                table: "AggregationRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationRequests_ProjectPhaseId",
                table: "AggregationRequests",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationRequests_SewerGroupId",
                table: "AggregationRequests",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregationRequests");
        }
    }
}
