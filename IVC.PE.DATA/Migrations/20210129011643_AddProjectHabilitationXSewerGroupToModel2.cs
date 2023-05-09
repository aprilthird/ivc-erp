using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddProjectHabilitationXSewerGroupToModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectHabilitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    LocationCode = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectHabilitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectHabilitations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerGroupProjectHabilitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    SewerGroupId = table.Column<Guid>(nullable: true),
                    ProjectHabilitationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerGroupProjectHabilitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerGroupProjectHabilitations_ProjectHabilitations_ProjectHabilitationId",
                        column: x => x.ProjectHabilitationId,
                        principalTable: "ProjectHabilitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerGroupProjectHabilitations_SewerGroups_SewerGroupId",
                        column: x => x.SewerGroupId,
                        principalTable: "SewerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHabilitations_ProjectId",
                table: "ProjectHabilitations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupProjectHabilitations_ProjectHabilitationId",
                table: "SewerGroupProjectHabilitations",
                column: "ProjectHabilitationId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroupProjectHabilitations_SewerGroupId",
                table: "SewerGroupProjectHabilitations",
                column: "SewerGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerGroupProjectHabilitations");

            migrationBuilder.DropTable(
                name: "ProjectHabilitations");
        }
    }
}
