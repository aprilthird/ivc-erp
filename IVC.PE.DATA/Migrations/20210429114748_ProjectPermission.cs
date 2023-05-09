using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Permissions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ProjectId",
                table: "Permissions",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Projects_ProjectId",
                table: "Permissions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Projects_ProjectId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_ProjectId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Permissions");
        }
    }
}
