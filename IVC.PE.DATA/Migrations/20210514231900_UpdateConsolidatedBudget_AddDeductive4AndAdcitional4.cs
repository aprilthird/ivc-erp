using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateConsolidatedBudget_AddDeductive4AndAdcitional4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AdicionalAmount4",
                table: "ConsolidatedBudgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DeductiveAmount4",
                table: "ConsolidatedBudgets",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdicionalAmount4",
                table: "ConsolidatedBudgets");

            migrationBuilder.DropColumn(
                name: "DeductiveAmount4",
                table: "ConsolidatedBudgets");
        }
    }
}
