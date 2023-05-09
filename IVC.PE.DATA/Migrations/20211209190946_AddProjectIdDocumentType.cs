using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectIdDocumentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "DocumentTypes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_ProjectId",
                table: "DocumentTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTypes_Projects_ProjectId",
                table: "DocumentTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTypes_Projects_ProjectId",
                table: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_ProjectId",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DocumentTypes");
        }
    }
}
