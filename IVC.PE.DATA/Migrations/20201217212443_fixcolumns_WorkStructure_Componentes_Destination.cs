using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixcolumns_WorkStructure_Componentes_Destination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WorkStructures",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "WorkComponents",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Destinations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkStructures_ProjectId",
                table: "WorkStructures",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkComponents_ProjectId",
                table: "WorkComponents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_ProjectId",
                table: "Destinations",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Projects_ProjectId",
                table: "Destinations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkComponents_Projects_ProjectId",
                table: "WorkComponents",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkStructures_Projects_ProjectId",
                table: "WorkStructures",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Projects_ProjectId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkComponents_Projects_ProjectId",
                table: "WorkComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkStructures_Projects_ProjectId",
                table: "WorkStructures");

            migrationBuilder.DropIndex(
                name: "IX_WorkStructures_ProjectId",
                table: "WorkStructures");

            migrationBuilder.DropIndex(
                name: "IX_WorkComponents_ProjectId",
                table: "WorkComponents");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_ProjectId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkStructures");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkComponents");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Destinations");
        }
    }
}
