using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerDailyTask2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HoursPaidLeave",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "LaborSuspension",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NonAttendance",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UnPaidLeave",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursPaidLeave",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "LaborSuspension",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "NonAttendance",
                table: "WorkerDailyTasks");

            migrationBuilder.DropColumn(
                name: "UnPaidLeave",
                table: "WorkerDailyTasks");
        }
    }
}
