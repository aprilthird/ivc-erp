using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddContractualsColumns_FoldingBudgetWeeklyAdvanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ContractualEQ",
                table: "FoldingBudgetWeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMO",
                table: "FoldingBudgetWeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMaterials",
                table: "FoldingBudgetWeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualSubcontract",
                table: "FoldingBudgetWeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractualEQ",
                table: "FoldingBudgetWeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "ContractualMO",
                table: "FoldingBudgetWeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "ContractualMaterials",
                table: "FoldingBudgetWeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "ContractualSubcontract",
                table: "FoldingBudgetWeeklyAdvances");
        }
    }
}
