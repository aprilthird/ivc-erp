using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePayrollParamsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SCTRHealthFixed",
                table: "PayrollParameters",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SCTRPensionFixed",
                table: "PayrollParameters",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCTRHealthFixed",
                table: "PayrollParameters");

            migrationBuilder.DropColumn(
                name: "SCTRPensionFixed",
                table: "PayrollParameters");
        }
    }
}
