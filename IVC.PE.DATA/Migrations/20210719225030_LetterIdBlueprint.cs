using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class LetterIdBlueprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LetterId",
                table: "Blueprints",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blueprints_LetterId",
                table: "Blueprints",
                column: "LetterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blueprints_Letters_LetterId",
                table: "Blueprints",
                column: "LetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blueprints_Letters_LetterId",
                table: "Blueprints");

            migrationBuilder.DropIndex(
                name: "IX_Blueprints_LetterId",
                table: "Blueprints");

            migrationBuilder.DropColumn(
                name: "LetterId",
                table: "Blueprints");
        }
    }
}
