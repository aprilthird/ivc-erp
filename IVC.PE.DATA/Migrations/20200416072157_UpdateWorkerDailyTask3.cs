using Microsoft.EntityFrameworkCore.Migrations;

namespace IVC.PE.DATA.Migrations
{
    public partial class UpdateWorkerDailyTask3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursMedicalLeave",
                table: "WorkerDailyTasks");

            migrationBuilder.AddColumn<bool>(
                name: "MedicalLeave",
                table: "WorkerDailyTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalLeave",
                table: "WorkerDailyTasks");

            migrationBuilder.AddColumn<decimal>(
                name: "HoursMedicalLeave",
                table: "WorkerDailyTasks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
