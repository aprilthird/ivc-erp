using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontProjectPhaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFrontProjectPhases",
                table: "WorkFrontProjectPhases");

            migrationBuilder.DropIndex(
                name: "IX_WorkFrontProjectPhases_WorkFrontId",
                table: "WorkFrontProjectPhases");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkFrontProjectPhases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFrontProjectPhases",
                table: "WorkFrontProjectPhases",
                columns: new[] { "WorkFrontId", "ProjectPhaseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFrontProjectPhases",
                table: "WorkFrontProjectPhases");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "WorkFrontProjectPhases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFrontProjectPhases",
                table: "WorkFrontProjectPhases",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontProjectPhases_WorkFrontId",
                table: "WorkFrontProjectPhases",
                column: "WorkFrontId");
        }
    }
}
