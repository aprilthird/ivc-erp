using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollAuthorizationRequestAddedUsersIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayrollUserAuthId",
                table: "PayrollAuthorizationRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskUserAuth1Id",
                table: "PayrollAuthorizationRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskUserAuth2Id",
                table: "PayrollAuthorizationRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayrollUserAuthId",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "TaskUserAuth1Id",
                table: "PayrollAuthorizationRequests");

            migrationBuilder.DropColumn(
                name: "TaskUserAuth2Id",
                table: "PayrollAuthorizationRequests");
        }
    }
}
