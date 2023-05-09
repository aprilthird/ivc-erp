using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddSewerManifoldReferencesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewerManifoldReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SewerManifoldReviewId = table.Column<Guid>(nullable: false),
                    SewerManifoldExecutionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewerManifoldReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SewerManifoldReferences_SewerManifolds_SewerManifoldExecutionId",
                        column: x => x.SewerManifoldExecutionId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SewerManifoldReferences_SewerManifolds_SewerManifoldReviewId",
                        column: x => x.SewerManifoldReviewId,
                        principalTable: "SewerManifolds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldReferences_SewerManifoldExecutionId",
                table: "SewerManifoldReferences",
                column: "SewerManifoldExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_SewerManifoldReferences_SewerManifoldReviewId",
                table: "SewerManifoldReferences",
                column: "SewerManifoldReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewerManifoldReferences");
        }
    }
}
