using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateOCTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorEQ",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "CollaboratorMO",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "ContractualEQ",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "ContractualMO",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "ContractualMaterials",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "ContractualSubcontract",
                table: "OCBudgets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "OCBudgets");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OCBudgets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OCBudgets");

            migrationBuilder.AddColumn<double>(
                name: "CollaboratorEQ",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CollaboratorMO",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualEQ",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMO",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualMaterials",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContractualSubcontract",
                table: "OCBudgets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "OCBudgets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
