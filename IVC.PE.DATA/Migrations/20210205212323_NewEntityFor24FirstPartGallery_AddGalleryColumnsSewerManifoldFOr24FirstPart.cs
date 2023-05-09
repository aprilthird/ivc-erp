using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewEntityFor24FirstPartGallery_AddGalleryColumnsSewerManifoldFOr24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "For24FirstPartGalleries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor24FirstPartId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_For24FirstPartGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_For24FirstPartGalleries_SewerManifoldFor24FirstParts_SewerManifoldFor24FirstPartId",
                        column: x => x.SewerManifoldFor24FirstPartId,
                        principalTable: "SewerManifoldFor24FirstParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_For24FirstPartGalleries_SewerManifoldFor24FirstPartId",
                table: "For24FirstPartGalleries",
                column: "SewerManifoldFor24FirstPartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "For24FirstPartGalleries");
        }
    }
}
