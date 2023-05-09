using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class add_bool_columns_patterncalibration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Days15",
                table: "PatternCalibrationRenewals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Days30",
                table: "PatternCalibrationRenewals",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days15",
                table: "PatternCalibrationRenewals");

            migrationBuilder.DropColumn(
                name: "Days30",
                table: "PatternCalibrationRenewals");
        }
    }
}
