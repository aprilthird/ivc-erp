using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_BudgetInputEntity_AddMeteredAndParcialColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Metered",
                table: "BudgetInputs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Parcial",
                table: "BudgetInputs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metered",
                table: "BudgetInputs");

            migrationBuilder.DropColumn(
                name: "Parcial",
                table: "BudgetInputs");
        }
    }
}
