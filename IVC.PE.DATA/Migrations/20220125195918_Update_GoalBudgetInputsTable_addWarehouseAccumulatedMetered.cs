using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_GoalBudgetInputsTable_addWarehouseAccumulatedMetered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WarehouseAccumulatedMetered",
                table: "GoalBudgetInputs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseAccumulatedMetered",
                table: "GoalBudgetInputs");
        }
    }
}
