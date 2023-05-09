using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnsFillingLaboratorytest_NewEntityOriginType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OriginTypeFillingLaboratory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OriginTypeFLName = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginTypeFillingLaboratory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginTypeFillingLaboratory_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FillingLaboratoryTests_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests",
                column: "OriginTypeFillingLaboratoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginTypeFillingLaboratory_ProjectId",
                table: "OriginTypeFillingLaboratory",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratory_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests",
                column: "OriginTypeFillingLaboratoryId",
                principalTable: "OriginTypeFillingLaboratory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratory_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests");

            migrationBuilder.DropTable(
                name: "OriginTypeFillingLaboratory");

            migrationBuilder.DropIndex(
                name: "IX_FillingLaboratoryTests_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests");

            migrationBuilder.DropColumn(
                name: "OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests");
        }
    }
}
