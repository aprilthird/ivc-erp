using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BiddingWorkRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiddingBusinessId",
                table: "BiddingWorks");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorks_BusinessId",
                table: "BiddingWorks",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingWorks_Businesses_BusinessId",
                table: "BiddingWorks",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingWorks_Businesses_BusinessId",
                table: "BiddingWorks");

            migrationBuilder.DropIndex(
                name: "IX_BiddingWorks_BusinessId",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "BiddingWorks");

            migrationBuilder.AddColumn<Guid>(
                name: "BiddingBusinessId",
                table: "BiddingWorks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
