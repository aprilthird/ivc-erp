using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddRdpReportToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RdpReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: false),
                    ReportDate = table.Column<DateTime>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    InstalledPipe = table.Column<float>(nullable: false),
                    NumberOfOperators = table.Column<int>(nullable: false),
                    NumberOfOfficials = table.Column<int>(nullable: false),
                    NumberOfPawns = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RdpReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RdpReports_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RdpReports_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RdpReports_SewerGroupId",
                table: "RdpReports",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RdpReports_WorkFrontHeadId",
                table: "RdpReports",
                column: "WorkFrontHeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RdpReports");
        }
    }
}
