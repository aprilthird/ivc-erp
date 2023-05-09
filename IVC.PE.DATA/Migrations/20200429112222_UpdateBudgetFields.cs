using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateBudgetFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetGroup",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "GoalValue",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SaleValue",
                table: "Budgets");

            migrationBuilder.AddColumn<double>(
                name: "GoalAmmount",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Budgets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SaleAmmount",
                table: "Budgets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Budgets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalAmmount",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "SaleAmmount",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "BudgetGroup",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BudgetType",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "GoalValue",
                table: "Budgets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SaleValue",
                table: "Budgets",
                type: "float",
                nullable: true);
        }
    }
}
