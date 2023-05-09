using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class FromToNulldates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate5",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate2",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate3",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate4",
                table: "Businesses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate5",
                table: "Businesses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FromDate5",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate2",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate3",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate4",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ToDate5",
                table: "Businesses");
        }
    }
}
