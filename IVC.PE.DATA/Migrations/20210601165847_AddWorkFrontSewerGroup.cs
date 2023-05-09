using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddWorkFrontSewerGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkFrontSewerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFrontSewerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFrontSewerGroups_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFrontSewerGroups_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontSewerGroups_SewerGroupId",
                table: "WorkFrontSewerGroups",
                column: "SewerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFrontSewerGroups_WorkFrontId",
                table: "WorkFrontSewerGroups",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkFrontSewerGroups");
        }
    }
}
