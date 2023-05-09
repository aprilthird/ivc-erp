using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerManifoldAndLotToEntities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifolds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkFrontHeadId = table.Column<Guid>(nullable: false),
                    ExcecutionWithDitch = table.Column<bool>(nullable: false),
                    WorkFrontId = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    SewerBoxStart = table.Column<Guid>(nullable: false),
                    SewerBoxEnd = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifolds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifolds_WorkFrontHeads_WorkFrontHeadId",
                        column: x => x.WorkFrontHeadId,
                        principalTable: "WorkFrontHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifolds_WorkFronts_WorkFrontId",
                        column: x => x.WorkFrontId,
                        principalTable: "WorkFronts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SewerManifoldLots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DitchHeight = table.Column<double>(nullable: false),
                    DitchLevelPercent = table.Column<double>(nullable: false),
                    PipeDiameter = table.Column<double>(nullable: false),
                    PipeType = table.Column<int>(nullable: false),
                    DitchClass = table.Column<string>(nullable: true),
                    LengthBetweenAxles = table.Column<double>(nullable: false),
                    LengthOfPipeInstalled = table.Column<double>(nullable: false),
                    TerrainType = table.Column<int>(nullable: false),
                    LengthOfDigging = table.Column<double>(nullable: false),
                    PavementOf2In = table.Column<double>(nullable: false),
                    PavementOf3In = table.Column<double>(nullable: false),
                    PavementOf3InMixed = table.Column<double>(nullable: false),
                    PavementWidth = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldLots_SewerManifolds_SewerManifoldId",
                        column: x => x.SewerManifoldId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldLots_SewerManifoldId",
                table: "SewerManifoldLots",
                column: "SewerManifoldId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_WorkFrontHeadId",
                table: "SewerManifolds",
                column: "WorkFrontHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_WorkFrontId",
                table: "SewerManifolds",
                column: "WorkFrontId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldLots");

            migrationBuilder.DropTable(
                name: "SewerManifolds");
        }
    }
}
