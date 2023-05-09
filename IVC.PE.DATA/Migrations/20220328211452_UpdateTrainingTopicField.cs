using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateTrainingTopicField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingResultStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Color = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingResultStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingCategoryId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingTopics_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingTopics_TrainingCategories_TrainingCategoryId",
                        column: x => x.TrainingCategoryId,
                        principalTable: "TrainingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTopicFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingTopicId = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTopicFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingTopicFiles_TrainingTopics_TrainingTopicId",
                        column: x => x.TrainingTopicId,
                        principalTable: "TrainingTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTopicFiles_TrainingTopicId",
                table: "TrainingTopicFiles",
                column: "TrainingTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTopics_ProjectId",
                table: "TrainingTopics",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTopics_TrainingCategoryId",
                table: "TrainingTopics",
                column: "TrainingCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingResultStatus");

            migrationBuilder.DropTable(
                name: "TrainingTopicFiles");

            migrationBuilder.DropTable(
                name: "TrainingTopics");

            migrationBuilder.DropTable(
                name: "TrainingCategories");
        }
    }
}
