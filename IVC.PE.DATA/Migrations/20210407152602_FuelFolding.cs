using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FuelFolding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelProviderFoldings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FuelProviderId = table.Column<Guid>(nullable: false),
                    CisternPlate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelProviderFoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelProviderFoldings_FuelProviders_FuelProviderId",
                        column: x => x.FuelProviderId,
                        principalTable: "FuelProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelProviderFoldings_FuelProviderId",
                table: "FuelProviderFoldings",
                column: "FuelProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelProviderFoldings");
        }
    }
}
