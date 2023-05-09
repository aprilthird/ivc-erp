using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class WorkerEmailConfirmationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailAlertSentDateTime",
                table: "Workers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationDateTime",
                table: "Workers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Workers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAlertSentDateTime",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationDateTime",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Workers");
        }
    }
}
