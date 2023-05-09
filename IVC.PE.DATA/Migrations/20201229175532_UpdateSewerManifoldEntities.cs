using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateSewerManifoldEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropTable(
                name: "SewerManifoldLots");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxEndId",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "SewerBoxStartId",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<string>(
                name: "DitchClass",
                table: "SewerManifolds",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DitchHeight",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DitchLevelPercent",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LengthBetweenAxles",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LengthOfDigging",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LengthOfPipeInstalled",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SewerManifolds",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PipeDiameter",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PipeType",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TerrainType",
                table: "SewerManifolds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DitchClass",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "DitchHeight",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "DitchLevelPercent",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "LengthBetweenAxles",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "LengthOfDigging",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "LengthOfPipeInstalled",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipeDiameter",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "PipeType",
                table: "SewerManifolds");

            migrationBuilder.DropColumn(
                name: "TerrainType",
                table: "SewerManifolds");

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxEndId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SewerBoxStartId",
                table: "SewerManifolds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SewerManifoldLots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DitchClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DitchHeight = table.Column<double>(type: "float", nullable: false),
                    DitchLevelPercent = table.Column<double>(type: "float", nullable: false),
                    LengthBetweenAxles = table.Column<double>(type: "float", nullable: false),
                    LengthOfDigging = table.Column<double>(type: "float", nullable: false),
                    LengthOfPipeInstalled = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PipeDiameter = table.Column<double>(type: "float", nullable: false),
                    PipeType = table.Column<int>(type: "int", nullable: false),
                    SewerManifoldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerrainType = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_SewerManifolds_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifolds_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldLots_SewerManifoldId",
                table: "SewerManifoldLots",
                column: "SewerManifoldId");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxEndId",
                table: "SewerManifolds",
                column: "SewerBoxEndId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SewerManifolds_SewerBoxes_SewerBoxStartId",
                table: "SewerManifolds",
                column: "SewerBoxStartId",
                principalTable: "SewerBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
