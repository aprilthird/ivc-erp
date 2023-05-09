using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NullableCollegeProfessional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollegeId",
                table: "Professionals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_CollegeId",
                table: "Professionals",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_Colleges_CollegeId",
                table: "Professionals",
                column: "CollegeId",
                principalTable: "Colleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_Colleges_CollegeId",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_CollegeId",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "CollegeId",
                table: "Professionals");
        }
    }
}
