using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewEntitySewerManifoldFor29AndAddColumnSewerManifoldFor05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "SewerManifoldFor05s",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerManifoldFor29s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    For01ProtocolNumber = table.Column<string>(nullable: true),
                    AsphaltDate = table.Column<DateTime>(nullable: false),
                    AlphaltType = table.Column<string>(nullable: true),
                    Thickness = table.Column<double>(nullable: false),
                    AsphaltArea = table.Column<double>(nullable: false),
                    AreaToValue = table.Column<double>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor29s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor29s_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor29s_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor05s_ProjectId",
                table: "SewerManifoldFor05s",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor29s_ProjectId",
                table: "SewerManifoldFor29s",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor29s_SewerManifoldId",
                table: "SewerManifoldFor29s",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifoldFor05s_Projects_ProjectId",
                table: "SewerManifoldFor05s",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifoldFor05s_Projects_ProjectId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropTable(
                name: "SewerManifoldFor29s");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifoldFor05s_ProjectId",
                table: "SewerManifoldFor05s");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "SewerManifoldFor05s");
        }
    }
}
