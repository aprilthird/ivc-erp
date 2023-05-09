using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_FoldingMeteredsRestatedByPartidas_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoldingMeteredsRestatedByPartidas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MeteredsRestatedByPartidaId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    Metered = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldingMeteredsRestatedByPartidas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldingMeteredsRestatedByPartidas_MeteredsRestatedByPartidas_MeteredsRestatedByPartidaId",
                        column: x => x.MeteredsRestatedByPartidaId,
                        principalTable: "MeteredsRestatedByPartidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoldingMeteredsRestatedByPartidas_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldingMeteredsRestatedByPartidas_MeteredsRestatedByPartidaId",
                table: "FoldingMeteredsRestatedByPartidas",
                column: "MeteredsRestatedByPartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_FoldingMeteredsRestatedByPartidas_SewerGroupId",
                table: "FoldingMeteredsRestatedByPartidas",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldingMeteredsRestatedByPartidas");
        }
    }
}
