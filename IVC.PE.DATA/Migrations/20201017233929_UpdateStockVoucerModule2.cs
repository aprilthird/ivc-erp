using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateStockVoucerModule2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockVouchers_Stocks_StockId",
                table: "StockVouchers");

            migrationBuilder.DropIndex(
                name: "IX_StockVouchers_StockId",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "SalePriceUnit",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "StockVouchers");

            migrationBuilder.CreateTable(
                name: "StockVoucherDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockVoucherId = table.Column<Guid>(nullable: false),
                    StockId = table.Column<Guid>(nullable: false),
                    CurrencyType = table.Column<int>(nullable: true),
                    SalePriceUnit = table.Column<decimal>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockVoucherDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockVoucherDetails_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockVoucherDetails_StockVouchers_StockVoucherId",
                        column: x => x.StockVoucherId,
                        principalTable: "StockVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockVoucherDetails_StockId",
                table: "StockVoucherDetails",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockVoucherDetails_StockVoucherId",
                table: "StockVoucherDetails",
                column: "StockVoucherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockVoucherDetails");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "StockVouchers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockVouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePriceUnit",
                table: "StockVouchers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockId",
                table: "StockVouchers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StockVouchers_StockId",
                table: "StockVouchers",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockVouchers_Stocks_StockId",
                table: "StockVouchers",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
