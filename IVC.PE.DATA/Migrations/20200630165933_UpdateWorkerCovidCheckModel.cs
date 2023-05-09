using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerCovidCheckModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HaveCovid",
                table: "WorkerCovidChecks");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "WorkerCovidChecks");

            migrationBuilder.AddColumn<int>(
                name: "IgG",
                table: "WorkerCovidChecks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IgM",
                table: "WorkerCovidChecks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestOutcome",
                table: "WorkerCovidChecks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestType",
                table: "WorkerCovidChecks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgG",
                table: "WorkerCovidChecks");

            migrationBuilder.DropColumn(
                name: "IgM",
                table: "WorkerCovidChecks");

            migrationBuilder.DropColumn(
                name: "TestOutcome",
                table: "WorkerCovidChecks");

            migrationBuilder.DropColumn(
                name: "TestType",
                table: "WorkerCovidChecks");

            migrationBuilder.AddColumn<bool>(
                name: "HaveCovid",
                table: "WorkerCovidChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "WorkerCovidChecks",
                type: "datetime2",
                nullable: true);
        }
    }
}
