using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Budget_Add_ContractualColumns_CollaboratorColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CollaboratorEQ",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CollaboratorMO",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualEQ",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMO",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMaterials",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualSubcontract",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorEQ",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CollaboratorMO",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ContractualEQ",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ContractualMO",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ContractualMaterials",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ContractualSubcontract",
                table: "Budgets");
        }
    }
}
