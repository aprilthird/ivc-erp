using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class CreateConsolidatedBudgetInputTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsolidatedBudgetInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplyFamilyId = table.Column<Guid>(nullable: false),
                    SupplyGroupId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_ConsolidatedBudgetInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsolidatedBudgetInputs_SupplyFamilies_SupplyFamilyId",
                        column: x => x.SupplyFamilyId,
                        principalTable: "SupplyFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsolidatedBudgetInputs_SupplyGroups_SupplyGroupId",
                        column: x => x.SupplyGroupId,
                        principalTable: "SupplyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedBudgetInputs_SupplyFamilyId",
                table: "ConsolidatedBudgetInputs",
                column: "SupplyFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsolidatedBudgetInputs_SupplyGroupId",
                table: "ConsolidatedBudgetInputs",
                column: "SupplyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsolidatedBudgetInputs");
        }
    }
}
