using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddBudgetColumns_WeeklyAdvanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccumulatedBudget",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageAdvance",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectFormulaId",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "TotalNetBudget",
                table: "WeeklyAdvances",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyAdvances_ProjectFormulaId",
                table: "WeeklyAdvances",
                column: "ProjectFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyAdvances_ProjectFormulas_ProjectFormulaId",
                table: "WeeklyAdvances",
                column: "ProjectFormulaId",
                principalTable: "ProjectFormulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyAdvances_ProjectFormulas_ProjectFormulaId",
                table: "WeeklyAdvances");

            migrationBuilder.DropIndex(
                name: "IX_WeeklyAdvances_ProjectFormulaId",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "AccumulatedBudget",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "PercentageAdvance",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "ProjectFormulaId",
                table: "WeeklyAdvances");

            migrationBuilder.DropColumn(
                name: "TotalNetBudget",
                table: "WeeklyAdvances");
        }
    }
}
