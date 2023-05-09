using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerCategories_Projects_ProjectId",
                table: "WorkerCategories");

            migrationBuilder.DropIndex(
                name: "IX_WorkerCategories_ProjectId",
                table: "WorkerCategories");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkerCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WorkerCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkerCategories_ProjectId",
                table: "WorkerCategories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerCategories_Projects_ProjectId",
                table: "WorkerCategories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
