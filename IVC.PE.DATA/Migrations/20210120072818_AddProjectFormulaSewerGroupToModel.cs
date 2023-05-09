using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectFormulaSewerGroupToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFormulaSewerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false),
                    ProjectFormulaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFormulaSewerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaSewerGroups_ProjectFormulas_ProjectFormulaId",
                        column: x => x.ProjectFormulaId,
                        principalTable: "ProjectFormulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFormulaSewerGroups_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaSewerGroups_ProjectFormulaId",
                table: "ProjectFormulaSewerGroups",
                column: "ProjectFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormulaSewerGroups_SewerGroupId",
                table: "ProjectFormulaSewerGroups",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFormulaSewerGroups");
        }
    }
}
