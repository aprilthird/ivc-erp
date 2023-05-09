using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerGroupColumnsSewerManifoldFor24FirstPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24FirstParts_SewerManifoldId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "SewerManifoldId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerGroupId",
                table: "SewerManifoldFor24FirstParts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_SewerGroupId",
                table: "SewerManifoldFor24FirstParts",
                column: "SewerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_SewerGroups_SewerGroupId",
                table: "SewerManifoldFor24FirstParts",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_SewerGroups_SewerGroupId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24FirstParts_SewerGroupId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "SewerGroupId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_SewerManifolds_SewerManifoldId",
                table: "SewerManifoldFor24FirstParts",
                column: "SewerManifoldId",
                principalTable: "SewerManifolds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
