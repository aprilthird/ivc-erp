using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateColumnsMeteredRestatedByPartidaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccumulatedAmount",
                table: "MeteredsRestatedByPartidas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AccumulatedMetered",
                table: "MeteredsRestatedByPartidas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Parcial",
                table: "MeteredsRestatedByPartidas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "MeteredsRestatedByPartidas",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccumulatedAmount",
                table: "MeteredsRestatedByPartidas");

            migrationBuilder.DropColumn(
                name: "AccumulatedMetered",
                table: "MeteredsRestatedByPartidas");

            migrationBuilder.DropColumn(
                name: "Parcial",
                table: "MeteredsRestatedByPartidas");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "MeteredsRestatedByPartidas");
        }
    }
}
