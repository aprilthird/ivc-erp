using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerSewerGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_WorkerId",
                table: "SewerGroups");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "ForemanId",
                table: "SewerGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups",
                column: "ForemanId",
                unique: true,
                filter: "[ForemanId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_ForemanId",
                table: "SewerGroups",
                column: "ForemanId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_SewerGroups_SewerGroupId",
                table: "Workers",
                column: "SewerGroupId",
                principalTable: "SewerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SewerGroups_Workers_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_SewerGroups_SewerGroupId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_SewerGroups_ForemanId",
                table: "SewerGroups");

            migrationBuilder.DropColumn(
                name: "ForemanId",
                table: "SewerGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerId",
                table: "SewerGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SewerGroups_WorkerId",
                table: "SewerGroups",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SewerGroups_Workers_WorkerId",
                table: "SewerGroups",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
