using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Create_FoldingBudgetWeeklyAdvanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoldingBudgetWeeklyAdvances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WeeklyAdvanceId = table.Column<Guid>(nullable: false),
                    NumberItem = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    ActualAdvance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldingBudgetWeeklyAdvances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldingBudgetWeeklyAdvances_WeeklyAdvances_WeeklyAdvanceId",
                        column: x => x.WeeklyAdvanceId,
                        principalTable: "WeeklyAdvances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldingBudgetWeeklyAdvances_WeeklyAdvanceId",
                table: "FoldingBudgetWeeklyAdvances",
                column: "WeeklyAdvanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldingBudgetWeeklyAdvances");
        }
    }
}
