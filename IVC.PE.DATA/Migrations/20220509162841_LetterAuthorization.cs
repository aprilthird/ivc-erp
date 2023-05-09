using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LetterAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LetterAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LetterId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterAuthorizations_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetterAuthorizations_LetterId",
                table: "LetterAuthorizations",
                column: "LetterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterAuthorizations");
        }
    }
}
