using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRdpItemModule3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RdpItemFootages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    RdpItemId = table.Column<Guid>(nullable: false),
                    RdpDate = table.Column<DateTime>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: true),
                    Contractual = table.Column<decimal>(nullable: true),
                    StakeOut = table.Column<decimal>(nullable: true),
                    Accumulated = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdpItemFootages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdpItemFootages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpItemFootages_RdpItems_RdpItemId",
                        column: x => x.RdpItemId,
                        principalTable: "RdpItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpItemFootages_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RdpItemTotals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    RdpItemId = table.Column<Guid>(nullable: false),
                    Contractual = table.Column<decimal>(nullable: true),
                    StakeOut = table.Column<decimal>(nullable: true),
                    Accumulated = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdpItemTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdpItemTotals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpItemTotals_RdpItems_RdpItemId",
                        column: x => x.RdpItemId,
                        principalTable: "RdpItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_ProjectId",
                table: "RdpItemFootages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_RdpItemId",
                table: "RdpItemFootages",
                column: "RdpItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemFootages_SewerGroupId",
                table: "RdpItemFootages",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemTotals_ProjectId",
                table: "RdpItemTotals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpItemTotals_RdpItemId",
                table: "RdpItemTotals",
                column: "RdpItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RdpItemFootages");

            migrationBuilder.DropTable(
                name: "RdpItemTotals");
        }
    }
}
