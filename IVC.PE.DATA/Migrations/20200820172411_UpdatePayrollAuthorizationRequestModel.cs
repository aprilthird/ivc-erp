using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollAuthorizationRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.AddColumn<bool>(
                name: "isPayrollOk",
                table: "PayrollAuthorizationRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPayrollOk",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "PayrollAuthorizationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "PayrollAuthorizationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "PayrollAuthorizationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
