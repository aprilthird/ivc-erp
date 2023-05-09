using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRequestAddIssuedUserProp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blueprint",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CalibrationCertificate",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Catalog",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Other",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherDescription",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "QualityCertificate",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TechnicalInformation",
                table: "Requests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blueprint",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CalibrationCertificate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Catalog",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OtherDescription",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "QualityCertificate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TechnicalInformation",
                table: "Requests");
        }
    }
}
