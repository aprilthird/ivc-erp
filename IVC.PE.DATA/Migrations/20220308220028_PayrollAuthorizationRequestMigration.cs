using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class PayrollAuthorizationRequestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPayrollOk",
                table: "PayrollAuthorizationRequests",
                newName: "IsPayrollOk");

            migrationBuilder.AddColumn<bool>(
                name: "PayrollAuthRequested",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserAnswered1",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserAnswered2",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayrollAuthRequested",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "UserAnswered1",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "UserAnswered2",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.RenameColumn(
                name: "IsPayrollOk",
                table: "PayrollAuthorizationRequests",
                newName: "isPayrollOk");
        }
    }
}
