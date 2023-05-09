using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddDiscountsInOrderItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AdditionalDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FinancialDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IGV",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ISC",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ItemDiscount",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FinancialDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IGV",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ISC",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemDiscount",
                table: "OrderItems");
        }
    }
}
