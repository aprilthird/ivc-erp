using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BluePrintFoldingDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlueprintTypeId",
                table: "Blueprints",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BluePrintFoldingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BlueprintFoldingId = table.Column<Guid>(nullable: false),
                    DateType = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BluePrintFoldingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BluePrintFoldingDetails_BlueprintFoldings_BlueprintFoldingId",
                        column: x => x.BlueprintFoldingId,
                        principalTable: "BlueprintFoldings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlueprintTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlueprintTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlueprintTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_BlueprintTypeId",
                table: "Blueprints",
                column: "BlueprintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BluePrintFoldingDetails_BlueprintFoldingId",
                table: "BluePrintFoldingDetails",
                column: "BlueprintFoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_BlueprintTypes_ProjectId",
                table: "BlueprintTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_BlueprintTypes_BlueprintTypeId",
                table: "Blueprints",
                column: "BlueprintTypeId",
                principalTable: "BlueprintTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_BlueprintTypes_BlueprintTypeId",
                table: "Blueprints");

            migrationBuilder.DropTable(
                name: "BluePrintFoldingDetails");

            migrationBuilder.DropTable(
                name: "BlueprintTypes");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_BlueprintTypeId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "BlueprintTypeId",
                table: "Blueprints");
        }
    }
}
