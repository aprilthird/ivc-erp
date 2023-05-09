using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerDailyTaskAddedCeasedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CeasedDate",
                table: "WorkerDailyTasks",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCeased",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CeasedDate",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "IsCeased",
                table: "WorkerDailyTasks");
        }
    }
}
