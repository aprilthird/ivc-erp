using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_SteelsTable_Add_WorkFrontAndProjectPhaseFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPhaseId",
                table: "Steels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "Steels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Steels_ProjectPhaseId",
                table: "Steels",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Steels_WorkFrontId",
                table: "Steels",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_Steels_ProjectPhases_ProjectPhaseId",
                table: "Steels",
                column: "ProjectPhaseId",
                principalTable: "ProjectPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Steels_WorkFronts_WorkFrontId",
                table: "Steels",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steels_ProjectPhases_ProjectPhaseId",
                table: "Steels");

            migrationBuilder.DropForeignKey(
                name: "FK_Steels_WorkFronts_WorkFrontId",
                table: "Steels");

            migrationBuilder.DropIndex(
                name: "IX_Steels_ProjectPhaseId",
                table: "Steels");

            migrationBuilder.DropIndex(
                name: "IX_Steels_WorkFrontId",
                table: "Steels");

            migrationBuilder.DropColumn(
                name: "ProjectPhaseId",
                table: "Steels");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "Steels");
        }
    }
}
