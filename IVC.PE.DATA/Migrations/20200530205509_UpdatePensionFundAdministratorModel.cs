using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePensionFundAdministratorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DisabilityInsuranceRate",
                table: "PensionFundAdministrators",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EarlyRetirementRate",
                table: "PensionFundAdministrators",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FlowComissionRate",
                table: "PensionFundAdministrators",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FundRate",
                table: "PensionFundAdministrators",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MixedComissionRate",
                table: "PensionFundAdministrators",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisabilityInsuranceRate",
                table: "PensionFundAdministrators");

            migrationBuilder.DropColumn(
                name: "EarlyRetirementRate",
                table: "PensionFundAdministrators");

            migrationBuilder.DropColumn(
                name: "FlowComissionRate",
                table: "PensionFundAdministrators");

            migrationBuilder.DropColumn(
                name: "FundRate",
                table: "PensionFundAdministrators");

            migrationBuilder.DropColumn(
                name: "MixedComissionRate",
                table: "PensionFundAdministrators");
        }
    }
}
