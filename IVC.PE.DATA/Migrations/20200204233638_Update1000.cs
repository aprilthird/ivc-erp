using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update1000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Letters_ReferenceId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_ReferenceId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "DocumentTypes",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Letters");

            migrationBuilder.AddColumn<string>(
                name: "Acronym",
                table: "IssuerTargets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LetterLetters",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(nullable: false),
                    ReferenceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterLetters", x => new { x.LetterId, x.ReferenceId });
                    table.ForeignKey(
                        name: "FK_LetterLetters_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LetterLetters_Letters_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LetterStatus",
                columns: table => new
                {
                    LetterId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterStatus", x => new { x.LetterId, x.Status });
                    table.ForeignKey(
                        name: "FK_LetterStatus_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetterLetters_ReferenceId",
                table: "LetterLetters",
                column: "ReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterLetters");

            migrationBuilder.DropTable(
                name: "LetterStatus");

            migrationBuilder.DropColumn(
                name: "Acronym",
                table: "IssuerTargets");

            migrationBuilder.AddColumn<string>(
                name: "DocumentTypes",
                table: "Letters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceId",
                table: "Letters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Letters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Letters_ReferenceId",
                table: "Letters",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Letters_ReferenceId",
                table: "Letters",
                column: "ReferenceId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
