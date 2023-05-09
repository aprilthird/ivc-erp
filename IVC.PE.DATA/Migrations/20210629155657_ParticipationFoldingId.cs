using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class ParticipationFoldingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessParticipationFoldingId",
                table: "BiddingWorks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorks_BusinessParticipationFoldingId",
                table: "BiddingWorks",
                column: "BusinessParticipationFoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingWorks_BusinessParticipationFoldings_BusinessParticipationFoldingId",
                table: "BiddingWorks",
                column: "BusinessParticipationFoldingId",
                principalTable: "BusinessParticipationFoldings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingWorks_BusinessParticipationFoldings_BusinessParticipationFoldingId",
                table: "BiddingWorks");

            migrationBuilder.DropIndex(
                name: "IX_BiddingWorks_BusinessParticipationFoldingId",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "BusinessParticipationFoldingId",
                table: "BiddingWorks");
        }
    }
}
