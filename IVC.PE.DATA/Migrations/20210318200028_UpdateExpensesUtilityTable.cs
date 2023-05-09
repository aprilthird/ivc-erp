using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateExpensesUtilityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalGeneralExpense",
                table: "ExpensesUtilities",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalGeneralPercentage",
                table: "ExpensesUtilities",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGeneralExpense",
                table: "ExpensesUtilities");

            migrationBuilder.DropColumn(
                name: "TotalGeneralPercentage",
                table: "ExpensesUtilities");
        }
    }
}
