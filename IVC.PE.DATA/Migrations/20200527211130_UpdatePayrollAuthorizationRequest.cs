using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollAuthorizationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WeeklyPayrollAuth",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WeeklyTaskAuth1",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WeeklyTaskAuth2",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyPayrollAuth",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "WeeklyTaskAuth1",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "WeeklyTaskAuth2",
                table: "PayrollAuthorizationRequests");
        }
    }
}
