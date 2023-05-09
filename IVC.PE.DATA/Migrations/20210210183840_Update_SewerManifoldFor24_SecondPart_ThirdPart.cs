using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SewerManifoldFor24_SecondPart_ThirdPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24ThirdParts_SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts",
                column: "SewerManifoldFor24SecondPartId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24SecondParts_SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts",
                column: "SewerManifoldFor24FirstPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24SecondParts_SewerManifoldFor24FirstParts_SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts",
                column: "SewerManifoldFor24FirstPartId",
                principalTable: "SewerManifoldFor24FirstParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24ThirdParts_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts",
                column: "SewerManifoldFor24SecondPartId",
                principalTable: "SewerManifoldFor24SecondParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24SecondParts_SewerManifoldFor24FirstParts_SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24ThirdParts_SewerManifoldFor24SecondParts_SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24ThirdParts_SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24SecondParts_SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts");

            migrationBuilder.DropColumn(
                name: "SewerManifoldFor24SecondPartId",
                table: "SewerManifoldFor24ThirdParts");

            migrationBuilder.DropColumn(
                name: "SewerManifoldFor24FirstPartId",
                table: "SewerManifoldFor24SecondParts");
        }
    }
}
