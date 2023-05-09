using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Procedures",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_ProjectId",
                table: "Procedures",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Projects_ProjectId",
                table: "Procedures",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Projects_ProjectId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_ProjectId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Procedures");
        }
    }
}
