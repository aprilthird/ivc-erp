using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasEPS",
                table: "Workers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSctr",
                table: "Workers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasUnionFee",
                table: "Workers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWeeklySettlement",
                table: "Workers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "JudicialRetentionFixedAmmount",
                table: "Workers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "JudicialRetentionPercentRate",
                table: "Workers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LaborRegimen",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SctrHealthType",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SctrPensionType",
                table: "Workers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasEPS",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasSctr",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasUnionFee",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "HasWeeklySettlement",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "JudicialRetentionFixedAmmount",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "JudicialRetentionPercentRate",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "LaborRegimen",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "SctrHealthType",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "SctrPensionType",
                table: "Workers");
        }
    }
}
