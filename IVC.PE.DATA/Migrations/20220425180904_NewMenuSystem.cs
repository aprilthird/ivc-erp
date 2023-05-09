using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class NewMenuSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkRoleId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkRoleId",
                table: "AspNetUsers",
                column: "WorkRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkRoles_WorkRoleId",
                table: "AspNetUsers",
                column: "WorkRoleId",
                principalTable: "WorkRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkRoles_WorkRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkRoleId",
                table: "AspNetUsers");
        }
    }
}
