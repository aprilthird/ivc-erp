using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewEntities_SewerManifoldFor37A_FoldingFor37A : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldFor37As",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    For01ProtocolNumber = table.Column<string>(nullable: true),
                    HotMeltsNumber = table.Column<int>(nullable: false),
                    ElectrofusionsNumber = table.Column<int>(nullable: false),
                    StartElectrofusionDate = table.Column<DateTime>(nullable: false),
                    EndElectrofusionDate = table.Column<DateTime>(nullable: false),
                    FirstPipeBatch = table.Column<string>(nullable: true),
                    SecondPipeBatch = table.Column<string>(nullable: true),
                    ThridPipeBatch = table.Column<string>(nullable: true),
                    ForthPipeBatch = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldFor37As", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor37As_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldFor37As_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FoldingFor37As",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldFor37AId = table.Column<Guid>(nullable: false),
                    WeldingType = table.Column<int>(nullable: false),
                    MeetingNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldingFor37As", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldingFor37As_SewerManifoldFor37As_SewerManifoldFor37AId",
                        column: x => x.SewerManifoldFor37AId,
                        principalTable: "SewerManifoldFor37As",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldingFor37As_SewerManifoldFor37AId",
                table: "FoldingFor37As",
                column: "SewerManifoldFor37AId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor37As_ProjectId",
                table: "SewerManifoldFor37As",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldFor37As_SewerManifoldId",
                table: "SewerManifoldFor37As",
                column: "SewerManifoldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldingFor37As");

            migrationBuilder.DropTable(
                name: "SewerManifoldFor37As");
        }
    }
}
