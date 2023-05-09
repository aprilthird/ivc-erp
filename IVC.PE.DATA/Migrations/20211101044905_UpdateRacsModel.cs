using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateRacsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SAQ20",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ21",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ22",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ23",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ24",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ25",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ26",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ27",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ28",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ29",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ30",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ31",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SAQ32",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SCQ27",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SCQ28",
                table: "RacsReports",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SAQ20",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ21",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ22",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ23",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ24",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ25",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ26",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ27",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ28",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ29",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ30",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ31",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SAQ32",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SCQ27",
                table: "RacsReports");

            migrationBuilder.DropColumn(
                name: "SCQ28",
                table: "RacsReports");
        }
    }
}
