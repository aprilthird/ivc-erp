using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_OrdersTable_addExchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "SupplyEntries");

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "SupplyEntries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
