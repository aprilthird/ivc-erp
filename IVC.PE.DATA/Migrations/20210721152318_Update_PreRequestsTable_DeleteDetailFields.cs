using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class Update_PreRequestsTable_DeleteDetailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blueprint",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "CalibrationCertificate",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "Catalog",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "OtherDescription",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "QualityCertificate",
                table: "PreRequests");

            migrationBuilder.DropColumn(
                name: "TechnicalInformation",
                table: "PreRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blueprint",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CalibrationCertificate",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Catalog",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Other",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherDescription",
                table: "PreRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "QualityCertificate",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TechnicalInformation",
                table: "PreRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
