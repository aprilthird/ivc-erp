using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBIMTechnicalOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IvcParticipation",
                table: "BiddingWorks");

            migrationBuilder.CreateTable(
                name: "Bims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bims_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bims_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bims_ProjectFormulaId",
                table: "Bims",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bims_WorkFrontId",
                table: "Bims",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bims");

            migrationBuilder.AddColumn<double>(
                name: "IvcParticipation",
                table: "BiddingWorks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
