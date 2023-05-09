using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdatePensionFundAddSunatCodeProp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SunatCode",
                table: "PayrollPensionFundAdministratorRates");

            migrationBuilder.AddColumn<string>(
                name: "SunatCode",
                table: "PensionFundAdministrators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SunatCode",
                table: "PensionFundAdministrators");

            migrationBuilder.AddColumn<string>(
                name: "SunatCode",
                table: "PayrollPensionFundAdministratorRates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
