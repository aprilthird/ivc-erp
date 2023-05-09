using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBondAddProjectResponsibleToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondAddProjectResponsibles_BondAdds_BondAddId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.DropIndex(
                name: "IX_BondAddProjectResponsibles_BondAddId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.DropColumn(
                name: "BondAddId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "BondAddProjectResponsibles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondAddProjectResponsibles_ProjectId",
                table: "BondAddProjectResponsibles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondAddProjectResponsibles_Projects_ProjectId",
                table: "BondAddProjectResponsibles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BondAddProjectResponsibles_Projects_ProjectId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.DropIndex(
                name: "IX_BondAddProjectResponsibles_ProjectId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "BondAddProjectResponsibles");

            migrationBuilder.AddColumn<Guid>(
                name: "BondAddId",
                table: "BondAddProjectResponsibles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BondAddProjectResponsibles_BondAddId",
                table: "BondAddProjectResponsibles",
                column: "BondAddId");

            migrationBuilder.AddForeignKey(
                name: "FK_BondAddProjectResponsibles_BondAdds_BondAddId",
                table: "BondAddProjectResponsibles",
                column: "BondAddId",
                principalTable: "BondAdds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
