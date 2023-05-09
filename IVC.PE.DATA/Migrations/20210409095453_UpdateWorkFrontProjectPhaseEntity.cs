using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkFrontProjectPhaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFormulaWorkFronts");

            migrationBuilder.DropColumn(
                name: "PorjectPhaseId",
                table: "WorkFrontProjectPhases");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectPhaseId",
                table: "WorkFrontProjectPhases",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectPhaseId",
                table: "WorkFrontProjectPhases",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "PorjectPhaseId",
                table: "WorkFrontProjectPhases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectFormulaWorkFronts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectFormulaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFrontId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFormulaWorkFronts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaWorkFronts_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaWorkFronts_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaWorkFronts_ProjectFormulaId",
                table: "ProjectFormulaWorkFronts",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaWorkFronts_WorkFrontId",
                table: "ProjectFormulaWorkFronts",
                column: "WorkFrontId");
        }
    }
}
