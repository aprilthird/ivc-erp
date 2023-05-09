using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_ExpensesUtilityTable_FileColumn_WeeklyAdvance_SubcontractAndMaterialsColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CostMaterials",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CostSubcontract",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GoalMaterials",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GoalSubcontract",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SaleMaterials",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SaleSubcontract",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "ExpensesUtilities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostMaterials",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "CostSubcontract",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "GoalMaterials",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "GoalSubcontract",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "SaleMaterials",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "SaleSubcontract",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "ExpensesUtilities");
        }
    }
}
