using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateMeteredRestatedByStreetchEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "MeteredsRestatedByStreetchs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MeteredsRestatedByStreetchs_ProjectId",
                table: "MeteredsRestatedByStreetchs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_Projects_ProjectId",
                table: "MeteredsRestatedByStreetchs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeteredsRestatedByStreetchs_Projects_ProjectId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropIndex(
                name: "IX_MeteredsRestatedByStreetchs_ProjectId",
                table: "MeteredsRestatedByStreetchs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "MeteredsRestatedByStreetchs");
        }
    }
}
