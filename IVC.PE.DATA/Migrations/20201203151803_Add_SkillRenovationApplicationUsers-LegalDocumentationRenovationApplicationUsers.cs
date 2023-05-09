using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Add_SkillRenovationApplicationUsersLegalDocumentationRenovationApplicationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalDocumentationRenovationApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LegalDocumentationRenovationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocumentationRenovationApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalDocumentationRenovationApplicationUsers_LegalDocumentationRenovations_LegalDocumentationRenovationId",
                        column: x => x.LegalDocumentationRenovationId,
                        principalTable: "LegalDocumentationRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillRenovationApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SkillRenovationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillRenovationApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillRenovationApplicationUsers_SkillRenovations_SkillRenovationId",
                        column: x => x.SkillRenovationId,
                        principalTable: "SkillRenovations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentationRenovationApplicationUsers_LegalDocumentationRenovationId",
                table: "LegalDocumentationRenovationApplicationUsers",
                column: "LegalDocumentationRenovationId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillRenovationApplicationUsers_SkillRenovationId",
                table: "SkillRenovationApplicationUsers",
                column: "SkillRenovationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDocumentationRenovationApplicationUsers");

            migrationBuilder.DropTable(
                name: "SkillRenovationApplicationUsers");
        }
    }
}
