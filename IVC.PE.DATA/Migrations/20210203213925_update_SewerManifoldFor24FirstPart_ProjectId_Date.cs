using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class update_SewerManifoldFor24FirstPart_ProjectId_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "SewerManifoldFor24FirstParts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerManifoldFor24FirstParts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor24FirstParts_ProjectId",
                table: "SewerManifoldFor24FirstParts",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Projects_ProjectId",
                table: "SewerManifoldFor24FirstParts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor24FirstParts_Projects_ProjectId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor24FirstParts_ProjectId",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "SewerManifoldFor24FirstParts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerManifoldFor24FirstParts");
        }
    }
}
