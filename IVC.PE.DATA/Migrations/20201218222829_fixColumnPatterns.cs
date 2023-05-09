using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class fixColumnPatterns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatternCalibrationRenewals");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatternCalibrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "PatternCalibrationRenewals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatternCalibrations");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "PatternCalibrationRenewals");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatternCalibrationRenewals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
