using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class addprojectwoorkbooktype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WorkbookTypes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkbookTypes_ProjectId",
                table: "WorkbookTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkbookTypes_Projects_ProjectId",
                table: "WorkbookTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkbookTypes_Projects_ProjectId",
                table: "WorkbookTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkbookTypes_ProjectId",
                table: "WorkbookTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkbookTypes");
        }
    }
}
