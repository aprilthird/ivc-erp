using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class skillskillrenovationcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfessionalId = table.Column<Guid>(nullable: false),
                    NumberOfRenovations = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillRenovations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SkillOrder = table.Column<int>(nullable: false),
                    SkillId = table.Column<Guid>(nullable: false),
                    DaysLimitTerm = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Days15 = table.Column<bool>(nullable: false),
                    Days30 = table.Column<bool>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    IsTheLast = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillRenovations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillRenovations_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillRenovations_SkillId",
                table: "SkillRenovations",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ProfessionalId",
                table: "Skills",
                column: "ProfessionalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillRenovations");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
