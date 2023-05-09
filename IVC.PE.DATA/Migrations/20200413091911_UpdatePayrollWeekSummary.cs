using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollWeekSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficialCollaboratorTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OfficialCollaboratorTotalHours",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OfficialHomeTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OfficialHomeTotalHours",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OperatorCollaboratorTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OperatorCollaboratorTotalHours",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OperatorHomeTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "OperatorHomeTotalHours",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "PawnCollaboratorTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "PawnCollaboratorTotalHours",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "PawnHomeTotalCosts",
                table: "PayrollWeekSummaries");

            migrationBuilder.DropColumn(
                name: "PawnHomeTotalHours",
                table: "PayrollWeekSummaries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OfficialCollaboratorTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OfficialCollaboratorTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OfficialHomeTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OfficialHomeTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatorCollaboratorTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatorCollaboratorTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatorHomeTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatorHomeTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PawnCollaboratorTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PawnCollaboratorTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PawnHomeTotalCosts",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PawnHomeTotalHours",
                table: "PayrollWeekSummaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
