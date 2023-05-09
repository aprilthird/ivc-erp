using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RequesteItemPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_ProjectPhases_ProjectPhaseId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_ProjectPhaseId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "RequestItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "RequestItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_ProjectPhaseId",
                table: "RequestItems",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_ProjectPhases_ProjectPhaseId",
                table: "RequestItems",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
