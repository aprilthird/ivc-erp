using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontPhasesRemoveFormulaPop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "SewerGroupId",
                table: "RacsReports",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "WorkFrontProjectPhases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SewerGroupId",
                table: "RacsReports",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

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
    }
}
