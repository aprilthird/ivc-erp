using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ProjectNullableBusinessId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BusinessId",
                table: "Projects",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Businesses_BusinessId",
                table: "Projects",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Businesses_BusinessId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_BusinessId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Projects");
        }
    }
}
