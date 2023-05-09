using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_RequestItemsTable_AddWorkFrontId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkFrontId",
                table: "RequestItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestItems_WorkFrontId",
                table: "RequestItems",
                column: "WorkFrontId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestItems_WorkFronts_WorkFrontId",
                table: "RequestItems",
                column: "WorkFrontId",
                principalTable: "WorkFronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestItems_WorkFronts_WorkFrontId",
                table: "RequestItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestItems_WorkFrontId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "WorkFrontId",
                table: "RequestItems");
        }
    }
}
