using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BlueprintWorkFront : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "Blueprints",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_WorkFrontId",
                table: "Blueprints",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_WorkFronts_WorkFrontId",
                table: "Blueprints",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_WorkFronts_WorkFrontId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_WorkFrontId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "Blueprints");
        }
    }
}
