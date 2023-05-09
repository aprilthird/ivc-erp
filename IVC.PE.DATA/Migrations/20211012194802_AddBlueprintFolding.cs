using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBlueprintFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_Letters_LetterId",
                table: "Blueprints");

            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_TechnicalVersions_TechnicalVersionId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_LetterId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_TechnicalVersionId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "LetterId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "QrString",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "TechnicalVersionId",
                table: "Blueprints");

            migrationBuilder.CreateTable(
                name: "BlueprintFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BlueprintId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    TechnicalVersionId = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    LetterId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlueprintFoldings_Blueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "Blueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlueprintFoldings_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlueprintFoldings_TechnicalVersions_TechnicalVersionId",
                        column: x => x.TechnicalVersionId,
                        principalTable: "TechnicalVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintFoldings_BlueprintId",
                table: "BlueprintFoldings",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintFoldings_LetterId",
                table: "BlueprintFoldings",
                column: "LetterId");

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintFoldings_TechnicalVersionId",
                table: "BlueprintFoldings",
                column: "TechnicalVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlueprintFoldings");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Blueprints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Blueprints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LetterId",
                table: "Blueprints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrString",
                table: "Blueprints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TechnicalVersionId",
                table: "Blueprints",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_LetterId",
                table: "Blueprints",
                column: "LetterId");

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_TechnicalVersionId",
                table: "Blueprints",
                column: "TechnicalVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_Letters_LetterId",
                table: "Blueprints",
                column: "LetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_TechnicalVersions_TechnicalVersionId",
                table: "Blueprints",
                column: "TechnicalVersionId",
                principalTable: "TechnicalVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
