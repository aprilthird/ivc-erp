using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class update_Professional_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Professionals");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfessionId",
                table: "Professionals",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ProfessionId",
                table: "Professionals",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_Professions_ProfessionId",
                table: "Professionals",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Professions_ProfessionId",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_ProfessionId",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "Professionals");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
