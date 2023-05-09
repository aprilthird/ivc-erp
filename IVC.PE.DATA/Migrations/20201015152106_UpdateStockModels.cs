using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateStockModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "StockVouchers",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "StockVouchers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoucherDate",
                table: "StockVouchers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CurrencyType",
                table: "Stocks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityMinimum",
                table: "Stocks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StockApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockApplicationUsers_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockApplicationUsers_StockId",
                table: "StockApplicationUsers",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "VoucherDate",
                table: "StockVouchers");

            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "QuantityMinimum",
                table: "Stocks");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "StockVouchers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
