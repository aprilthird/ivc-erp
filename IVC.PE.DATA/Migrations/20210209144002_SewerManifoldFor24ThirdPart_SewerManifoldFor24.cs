using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class SewerManifoldFor24ThirdPart_SewerManifoldFor24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldFor24ThirdParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActionTaken = table.Column<int>(nullable: false),
                    PreventiveCorrectiveAction = table.Column<int>(nullable: false),
                    ClosingDate = table.Column<DateTime>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor24ThirdParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SewerManifoldFor24s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24FirstPartId = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24SecondPartId = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24ThirdpartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor24s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24s_SewerManifoldFor24FirstParts_SewerManifoldFor24FirstPartId",
                        column: x => x.SewerManifoldFor24FirstPartId,
                        principalTable: "SewerManifoldFor24FirstParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24s_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                        column: x => x.SewerManifoldFor24SecondPartId,
                        principalTable: "SewerManifoldFor24SecondParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor24s_SewerManifoldFor24ThirdParts_SewerManifoldFor24ThirdpartId",
                        column: x => x.SewerManifoldFor24ThirdpartId,
                        principalTable: "SewerManifoldFor24ThirdParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24s_SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24s",
                column: "SewerManifoldFor24FirstPartId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24s_SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24s",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24s_SewerManifoldFor24ThirdpartId",
                table: "SewerManifoldFor24s",
                column: "SewerManifoldFor24ThirdpartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldFor24s");

            migrationBuilder.DropTable(
                name: "SewerManifoldFor24ThirdParts");
        }
    }
}
