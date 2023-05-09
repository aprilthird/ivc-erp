using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class RemoveBiddingWorkComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingWorks_BiddingWorkComponents_BiddingWorkComponentId",
                table: "BiddingWorks");

            migrationBuilder.DropIndex(
                name: "IX_BiddingWorks_BiddingWorkComponentId",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "BiddingWorkComponentId",
                table: "BiddingWorks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BiddingWorkComponentId",
                table: "BiddingWorks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorks_BiddingWorkComponentId",
                table: "BiddingWorks",
                column: "BiddingWorkComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingWorks_BiddingWorkComponents_BiddingWorkComponentId",
                table: "BiddingWorks",
                column: "BiddingWorkComponentId",
                principalTable: "BiddingWorkComponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
