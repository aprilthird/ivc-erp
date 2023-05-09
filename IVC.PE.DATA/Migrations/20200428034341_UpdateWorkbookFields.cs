using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkbookFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "WorkbookSeats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WorkbookSeats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Workbooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "WorkbookSeats");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkbookSeats");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Workbooks");
        }
    }
}
