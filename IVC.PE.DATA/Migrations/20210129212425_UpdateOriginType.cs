using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateOriginType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratory_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests");

            migrationBuilder.DropForeignKey(
                name: "FK_OriginTypeFillingLaboratory_Projects_ProjectId",
                table: "OriginTypeFillingLaboratory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OriginTypeFillingLaboratory",
                table: "OriginTypeFillingLaboratory");

            migrationBuilder.RenameTable(
                name: "OriginTypeFillingLaboratory",
                newName: "OriginTypeFillingLaboratories");

            migrationBuilder.RenameIndex(
                name: "IX_OriginTypeFillingLaboratory_ProjectId",
                table: "OriginTypeFillingLaboratories",
                newName: "IX_OriginTypeFillingLaboratories_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OriginTypeFillingLaboratories",
                table: "OriginTypeFillingLaboratories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratories_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests",
                column: "OriginTypeFillingLaboratoryId",
                principalTable: "OriginTypeFillingLaboratories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OriginTypeFillingLaboratories_Projects_ProjectId",
                table: "OriginTypeFillingLaboratories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratories_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests");

            migrationBuilder.DropForeignKey(
                name: "FK_OriginTypeFillingLaboratories_Projects_ProjectId",
                table: "OriginTypeFillingLaboratories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OriginTypeFillingLaboratories",
                table: "OriginTypeFillingLaboratories");

            migrationBuilder.RenameTable(
                name: "OriginTypeFillingLaboratories",
                newName: "OriginTypeFillingLaboratory");

            migrationBuilder.RenameIndex(
                name: "IX_OriginTypeFillingLaboratories_ProjectId",
                table: "OriginTypeFillingLaboratory",
                newName: "IX_OriginTypeFillingLaboratory_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OriginTypeFillingLaboratory",
                table: "OriginTypeFillingLaboratory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FillingLaboratoryTests_OriginTypeFillingLaboratory_OriginTypeFillingLaboratoryId",
                table: "FillingLaboratoryTests",
                column: "OriginTypeFillingLaboratoryId",
                principalTable: "OriginTypeFillingLaboratory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OriginTypeFillingLaboratory_Projects_ProjectId",
                table: "OriginTypeFillingLaboratory",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
