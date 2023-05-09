using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Projects_ProjectId",
                table: "Procedures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderCatalogs_Projects_ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalLibrarys_Projects_ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalLibrarys_ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropIndex(
                name: "IX_ProviderCatalogs_ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_ProjectId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TechnicalLibrarys");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProviderCatalogs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Procedures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TechnicalLibrarys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProviderCatalogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Procedures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalLibrarys_ProjectId",
                table: "TechnicalLibrarys",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderCatalogs_ProjectId",
                table: "ProviderCatalogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_ProjectId",
                table: "Procedures",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Projects_ProjectId",
                table: "Procedures",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderCatalogs_Projects_ProjectId",
                table: "ProviderCatalogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalLibrarys_Projects_ProjectId",
                table: "TechnicalLibrarys",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
