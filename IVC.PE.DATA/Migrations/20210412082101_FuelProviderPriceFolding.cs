using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelProviderPriceFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LastPrice",
                table: "FuelProviders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FuelProviderPriceFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FuelProviderId = table.Column<Guid>(nullable: false),
                    PublicationDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelProviderPriceFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelProviderPriceFoldings_FuelProviders_FuelProviderId",
                        column: x => x.FuelProviderId,
                        principalTable: "FuelProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelProviderPriceFoldings_FuelProviderId",
                table: "FuelProviderPriceFoldings",
                column: "FuelProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelProviderPriceFoldings");

            migrationBuilder.DropColumn(
                name: "LastPrice",
                table: "FuelProviders");
        }
    }
}
