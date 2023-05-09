using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectPhaseBluePrint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "Blueprints",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_ProjectPhaseId",
                table: "Blueprints",
                column: "ProjectPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_ProjectPhases_ProjectPhaseId",
                table: "Blueprints",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_ProjectPhases_ProjectPhaseId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_ProjectPhaseId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "Blueprints");
        }
    }
}
