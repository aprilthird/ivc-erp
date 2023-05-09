using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SewerManifoldFor24Secondpart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_For24FirstPartGalleries_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");

            migrationBuilder.DropIndex(
                name: "IX_For24FirstPartGalleries_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");

            migrationBuilder.DropColumn(
                name: "SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_For24FirstPartGalleries_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_For24FirstPartGalleries_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "For24FirstPartGalleries",
                column: "SewerManifoldFor24SecondPartId",
                principalTable: "SewerManifoldFor24SecondParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
