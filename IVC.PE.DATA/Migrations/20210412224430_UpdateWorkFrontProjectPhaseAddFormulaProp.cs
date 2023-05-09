using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontProjectPhaseAddFormulaProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "WorkFrontProjectPhases",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontProjectPhases_ProjectFormulaId",
                table: "WorkFrontProjectPhases",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFrontProjectPhases_ProjectFormulas_ProjectFormulaId",
                table: "WorkFrontProjectPhases",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFrontProjectPhases_ProjectFormulas_ProjectFormulaId",
                table: "WorkFrontProjectPhases");

            migrationBuilder.DropIndex(
                name: "IX_WorkFrontProjectPhases_ProjectFormulaId",
                table: "WorkFrontProjectPhases");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "WorkFrontProjectPhases");
        }
    }
}
