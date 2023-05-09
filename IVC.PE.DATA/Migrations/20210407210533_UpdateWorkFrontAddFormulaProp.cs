using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontAddFormulaProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "WorkFronts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFronts_ProjectFormulaId",
                table: "WorkFronts",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFronts_ProjectFormulas_ProjectFormulaId",
                table: "WorkFronts",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFronts_ProjectFormulas_ProjectFormulaId",
                table: "WorkFronts");

            migrationBuilder.DropIndex(
                name: "IX_WorkFronts_ProjectFormulaId",
                table: "WorkFronts");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "WorkFronts");
        }
    }
}
