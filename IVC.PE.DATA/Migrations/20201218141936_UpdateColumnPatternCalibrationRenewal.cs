using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnPatternCalibrationRenewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatternCalibrations");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatternCalibrationRenewals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatternCalibrationRenewals");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatternCalibrations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
