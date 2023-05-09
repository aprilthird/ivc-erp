using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FixBiddingWorkCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BiddingCurrencyTypeId",
                table: "BiddingWorks",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ContractDollarAmmount",
                table: "BiddingWorks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "BiddingWorks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LiquidationDollarAmmount",
                table: "BiddingWorks",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ParticipationDollarAmmount",
                table: "BiddingWorks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BiddingCurrencyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Currency = table.Column<double>(nullable: false),
                    PublicationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingCurrencyTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiddingWorks_BiddingCurrencyTypeId",
                table: "BiddingWorks",
                column: "BiddingCurrencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingWorks_BiddingCurrencyTypes_BiddingCurrencyTypeId",
                table: "BiddingWorks",
                column: "BiddingCurrencyTypeId",
                principalTable: "BiddingCurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingWorks_BiddingCurrencyTypes_BiddingCurrencyTypeId",
                table: "BiddingWorks");

            migrationBuilder.DropTable(
                name: "BiddingCurrencyTypes");

            migrationBuilder.DropIndex(
                name: "IX_BiddingWorks_BiddingCurrencyTypeId",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "BiddingCurrencyTypeId",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "ContractDollarAmmount",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "LiquidationDollarAmmount",
                table: "BiddingWorks");

            migrationBuilder.DropColumn(
                name: "ParticipationDollarAmmount",
                table: "BiddingWorks");
        }
    }
}
