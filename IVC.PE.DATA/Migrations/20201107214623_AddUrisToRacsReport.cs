using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class AddUrisToRacsReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "RacsReports");

            migrationBuilder.AddColumn<string>(
                name: "LiftingImageUrl",
                table: "RacsReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationImageUrl",
                table: "RacsReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservationImageUrl",
                table: "RacsReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureUrl",
                table: "RacsReports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiftingImageUrl",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "LocationImageUrl",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "ObservationImageUrl",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SignatureUrl",
                table: "RacsReports");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RacsReports",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
