using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateProjectPhaseAddFormulaProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "ProjectPhases",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhases_ProjectFormulaId",
                table: "ProjectPhases",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhases_ProjectFormulas_ProjectFormulaId",
                table: "ProjectPhases",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhases_ProjectFormulas_ProjectFormulaId",
                table: "ProjectPhases");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPhases_ProjectFormulaId",
                table: "ProjectPhases");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "ProjectPhases");
        }
    }
}
