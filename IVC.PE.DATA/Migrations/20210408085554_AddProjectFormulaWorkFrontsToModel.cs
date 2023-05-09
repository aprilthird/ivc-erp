using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectFormulaWorkFrontsToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ProjectFormulaWorkFronts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFormulaWorkFronts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "WorkFronts",
                type: "uniqueidentifier",
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
    }
}
