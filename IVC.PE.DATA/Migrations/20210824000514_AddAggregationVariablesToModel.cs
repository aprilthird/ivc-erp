using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddAggregationVariablesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregationEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AggregationProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LicensePlate = table.Column<string>(nullable: true),
                    Volume = table.Column<string>(nullable: true),
                    VolumeCertificate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AggregationProviderTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationProviderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AggregationStockTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationStockTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AggregationStocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AggregationStockTypeId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    QuarryApprovalCertificate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregationStocks_AggregationStockTypes_AggregationStockTypeId",
                        column: x => x.AggregationStockTypeId,
                        principalTable: "AggregationStockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AggregationPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AggregationStockTypeId = table.Column<Guid>(nullable: false),
                    AggregationStockId = table.Column<Guid>(nullable: false),
                    AggregationEntryId = table.Column<Guid>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggregationPrices_AggregationEntries_AggregationEntryId",
                        column: x => x.AggregationEntryId,
                        principalTable: "AggregationEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationPrices_AggregationStocks_AggregationStockId",
                        column: x => x.AggregationStockId,
                        principalTable: "AggregationStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AggregationPrices_AggregationStockTypes_AggregationStockTypeId",
                        column: x => x.AggregationStockTypeId,
                        principalTable: "AggregationStockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AggregationPrices_AggregationEntryId",
                table: "AggregationPrices",
                column: "AggregationEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationPrices_AggregationStockId",
                table: "AggregationPrices",
                column: "AggregationStockId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationPrices_AggregationStockTypeId",
                table: "AggregationPrices",
                column: "AggregationStockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AggregationStocks_AggregationStockTypeId",
                table: "AggregationStocks",
                column: "AggregationStockTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregationPrices");

            migrationBuilder.DropTable(
                name: "AggregationProviders");

            migrationBuilder.DropTable(
                name: "AggregationProviderTypes");

            migrationBuilder.DropTable(
                name: "AggregationEntries");

            migrationBuilder.DropTable(
                name: "AggregationStocks");

            migrationBuilder.DropTable(
                name: "AggregationStockTypes");
        }
    }
}
