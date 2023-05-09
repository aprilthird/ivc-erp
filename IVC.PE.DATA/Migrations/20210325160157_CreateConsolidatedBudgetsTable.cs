using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateConsolidatedBudgetsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedBudgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NumberItem = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ContractualAmount = table.Column<double>(nullable: false),
                    DeductiveAmount1 = table.Column<double>(nullable: false),
                    DeductiveAmount2 = table.Column<double>(nullable: false),
                    DeductiveAmount3 = table.Column<double>(nullable: false),
                    Deductives = table.Column<double>(nullable: false),
                    NetContractual = table.Column<double>(nullable: false),
                    AdicionalAmount1 = table.Column<double>(nullable: false),
                    AdicionalAmount2 = table.Column<double>(nullable: false),
                    AdicionalAmount3 = table.Column<double>(nullable: false),
                    Adicionals = table.Column<double>(nullable: false),
                    AccumulatedAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidatedBudgets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedBudgets");
        }
    }
}
