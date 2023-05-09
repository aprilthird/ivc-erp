using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UserNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkPositionId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkPositionId",
                table: "AspNetUsers",
                column: "WorkPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkPositions_WorkPositionId",
                table: "AspNetUsers",
                column: "WorkPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkPositions_WorkPositionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkPositionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkPositionId",
                table: "AspNetUsers");
        }
    }
}
