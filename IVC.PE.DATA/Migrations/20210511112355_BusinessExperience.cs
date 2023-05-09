using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class BusinessExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalExperienceFoldings_BiddingBusinesses_BiddingBusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalExperienceFoldings_BiddingBusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.DropColumn(
                name: "BiddingBusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "ProfessionalExperienceFoldings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_BusinessId",
                table: "ProfessionalExperienceFoldings",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalExperienceFoldings_Businesses_BusinessId",
                table: "ProfessionalExperienceFoldings",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalExperienceFoldings_Businesses_BusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalExperienceFoldings_BusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "ProfessionalExperienceFoldings");

            migrationBuilder.AddColumn<Guid>(
                name: "BiddingBusinessId",
                table: "ProfessionalExperienceFoldings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalExperienceFoldings_BiddingBusinessId",
                table: "ProfessionalExperienceFoldings",
                column: "BiddingBusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalExperienceFoldings_BiddingBusinesses_BiddingBusinessId",
                table: "ProfessionalExperienceFoldings",
                column: "BiddingBusinessId",
                principalTable: "BiddingBusinesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
