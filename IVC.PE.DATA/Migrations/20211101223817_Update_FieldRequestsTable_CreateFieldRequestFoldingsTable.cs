using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_FieldRequestsTable_CreateFieldRequestFoldingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentNumber",
                table: "FieldRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FieldRequestFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FieldRequestId = table.Column<Guid>(nullable: false),
                    SupplyId = table.Column<Guid>(nullable: false),
                    ProjectPhaseId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldRequestFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldRequestFoldings_FieldRequests_FieldRequestId",
                        column: x => x.FieldRequestId,
                        principalTable: "FieldRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequestFoldings_ProjectPhases_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldRequestFoldings_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestFoldings_FieldRequestId",
                table: "FieldRequestFoldings",
                column: "FieldRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestFoldings_ProjectPhaseId",
                table: "FieldRequestFoldings",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldRequestFoldings_SupplyId",
                table: "FieldRequestFoldings",
                column: "SupplyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldRequestFoldings");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "FieldRequests");
        }
    }
}
